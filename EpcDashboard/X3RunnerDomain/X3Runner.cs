using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Security;
using System.Windows;

namespace Epc.ExternalRunnerDomain
{
    /// <summary>
    /// Class responsible for configuring and invoking Empiri X3
    /// </summary>
    public class X3Runner : MarshalByRefObject
    {
        private string _assemblyDirectory;

        public static void RunInOtherDomain(string sourcePath, string assemblyFileName, string ip, string dbName, string dbUser, string dbPsw, string userId)
        {
            var ownType = typeof(X3Runner);
            string ownAssemblyName = ownType.Assembly.FullName;

            // setup new app domain
            AppDomainSetup setup = new AppDomainSetup();
            string name = Guid.NewGuid().ToString();
            setup.ApplicationName = name;
            // create new app domain
            AppDomain x3Domain = AppDomain.CreateDomain(name, null, setup);

            try
            {
                x3Domain.Load(ownAssemblyName);

                // Call Run() in other AppDomain.
                DbInfo dbInfo = new DbInfo { IpAdress = ip, Name = dbName, UserName = dbUser, Password = dbPsw };
                X3Runner runner = (X3Runner)x3Domain.CreateInstanceAndUnwrap(ownAssemblyName, ownType.FullName);

                runner.Run(sourcePath, assemblyFileName, dbInfo, userId);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            AppDomain.Unload(x3Domain);
        }

        public void Run(string sourcePath, string assemblyFileName, DbInfo dbInfo, string userId)
        {
            _assemblyDirectory = sourcePath;
            string assemblyPath = Path.Combine(sourcePath, assemblyFileName);
            Assembly assembly = Assembly.LoadFrom(assemblyPath);

            try
            {
                //Add Empiri X3 dependency resources to this appdomain
                CopyResources(_assemblyDirectory);
                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception message: {0}", e.Message);
                MessageBox.Show("Could not prepare Empiri X3, exception message: " + e.Message);
            }

            //Create sql session
            SqlConnection conn = ConnectSQL(dbInfo);
            try
            {
                Guid sessionGuid = StartSqlSession(assembly.GetName().Version.ToString(), dbInfo, conn);
                string plantRuid = SqlPlantsQuery(conn, dbInfo.Name);
                bool isEx3User = SqlUsersQuery(conn, userId);
                if (isEx3User)
                {
                    //Ready to start Empiri X3
                    ConfigureAndStartEx3(assembly, sessionGuid, dbInfo, plantRuid, userId);
                } else
                {
                    //Not a registered ex3user -display message
                    MessageBox.Show("Your Userid is not a registered Empiri X3 user.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception message: {0}", e.Message);
                MessageBox.Show("Could not start Empiri X3, exception message: " + e.Message);
            }
        }

        private void CopyResources(string _assemblyDirectory)
        {
            CopyLanguageResources(_assemblyDirectory);
        }

        /// <summary>
        /// <para></para>
        /// <para></para>
        /// </summary>
        private void CopyLanguageResources(string sourcePath)
        {
            string langResource = @"EX3.Language.sv-SE.xml";
            string appRoot = AppDomain.CurrentDomain.BaseDirectory;
            //Copy EX3.Language.sv-SE.xml to EPC assembly root, overwrite if already exists
            File.Copy(Path.Combine(sourcePath, langResource), Path.Combine(appRoot, langResource), true);
        }

        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly a = null;
            string[] tokens = args.Name.Split(",".ToCharArray());
            string fullFileName = tokens[0] + ".dll";
            System.Diagnostics.Debug.WriteLine("Resolving : " + args.Name);

            try
            {
                if (File.Exists(Path.Combine(this._assemblyDirectory, fullFileName)))
                {
                    var otherAssemblyBytes = File.ReadAllBytes(Path.Combine(this._assemblyDirectory, fullFileName));
                    a = AppDomain.CurrentDomain.Load(otherAssemblyBytes);
                }
                else if (fullFileName.EndsWith(".resources.dll"))
                {
                    fullFileName = fullFileName.Replace(".resources.dll", ".dll");
                    if (File.Exists(Path.Combine(this._assemblyDirectory, fullFileName)))
                    {
                        var otherAssemblyBytes = File.ReadAllBytes(Path.Combine(this._assemblyDirectory, fullFileName));
                        a = AppDomain.CurrentDomain.Load(otherAssemblyBytes);
                    }
                }
                else if (fullFileName.EndsWith(".dll"))
                {
                    SearchForExe(fullFileName);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception while resolving, msg: {0}", e.Message);
            }

            return a;
        }

        private Assembly SearchForExe(string fullFileName)
        {
            Assembly a = null;
            fullFileName = fullFileName.Replace(".dll", ".exe");
            if (File.Exists(Path.Combine(this._assemblyDirectory, fullFileName)))
            {
                var otherAssemblyBytes = File.ReadAllBytes(Path.Combine(this._assemblyDirectory, fullFileName));
                a = AppDomain.CurrentDomain.Load(otherAssemblyBytes);
            }

            return a;
        }

        private Guid StartSqlSession(string appVersion, DbInfo dbInfo, SqlConnection conn)
        {
            // Create user session
            conn.Open();
            DataTable result = new DataTable();
            SqlTransaction transaction = conn.BeginTransaction();
            SqlCommand sc = new SqlCommand(@"EX3_UserSession_Login", conn, transaction);
            sc.CommandType = CommandType.StoredProcedure;
            sc.Parameters.AddWithValue("@CLIE_Mcad", GetMacAddress());
            sc.Parameters.AddWithValue("@CLIE_Ipad", GetIpAddress());
            sc.Parameters.AddWithValue("@CLIE_Comp", GetComputerName());
            sc.Parameters.AddWithValue("@CLIE_Comm", "");
            sc.Parameters.AddWithValue("@USER_Usid", Environment.UserName);
            sc.Parameters.AddWithValue("@USER_Apve", appVersion);
            sc.Parameters.AddWithValue("@USES_Seid", System.Diagnostics.Process.GetCurrentProcess().SessionId);

            try
            {
                SqlDataAdapter sda = new SqlDataAdapter(sc);
                sda.Fill(result);
                // Attempt to commit the transaction.
                transaction.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);

                // Attempt to roll back the transaction.
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                    // This catch block will handle any errors that may have occurred
                    // on the server that would cause the rollback to fail, such as
                    // a closed connection.
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                }
            }

            conn.Close();

            return Guid.Parse(result.Rows[0]["USES_Guid"].ToString());
        }

        private SqlConnection ConnectSQL(DbInfo dbInfo)
        {
            SqlConnection conn = new SqlConnection();
            System.Security.SecureString securePsw = ConvertToSecureString(dbInfo.Password);
            securePsw.MakeReadOnly();
            try
            {
                SqlCredential sc = new SqlCredential(dbInfo.UserName, securePsw);
                string connectionString = "Data Source = " + dbInfo.IpAdress + "; Initial Catalog = " + dbInfo.Name + ";";
                conn = new SqlConnection(connectionString, sc);
            }
            catch (Exception e)
            {
                Console.WriteLine("Sql connection error, message: {0}", e.Message);
            }

            return conn;
        }

        /// <summary>
        /// <para></para>
        /// <para></para>
        /// </summary>
        private string SqlPlantsQuery(SqlConnection conn, string dbName)
        {
            string result = null;
            try
            {
                string sql = "SELECT PLAN_Ruid FROM PLANTS WHERE PLAN_Dbnm ='" + dbName + "'";
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    result = reader.GetValue(0).ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception while querying database for PLAN_Ruid, msg: {0}", ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        private bool SqlUsersQuery(SqlConnection conn, string userId)
        {
            bool result = false;
            try
            {
                string sql = "SELECT USER_Usid FROM USERS WHERE User_Usid ='" + userId + "'";
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    string resultRow = reader.GetValue(0).ToString();
                    if(!String.IsNullOrEmpty(resultRow))
                    {
                        result = true; 
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception while querying database for Users_Userid, msg: {0}", ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// <para></para>
        /// <para></para>
        /// </summary>
        private void ConfigureAndStartEx3(Assembly assembly, Guid sessionGuid, DbInfo dbInfo, string plan_Ruid, string userId)
        {
            Type type = GetEx3Type(assembly);
            String methodName = "EntryPoint";
            if (type != null && sessionGuid != null)
            {
                Object[] args = GetEx3Args(dbInfo, plan_Ruid, sessionGuid, userId);
                try
                {
                    MethodInfo mi = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static);
                    InvokeMethod(mi, args, assembly);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception MethodInfo, msg: {0}", e.Message);
                }
            }
        }

        private void InvokeMethod(MethodInfo mi, object[] args, Assembly assembly)
        {
            if (mi != null)
            {
                try
                {
                    mi.Invoke(null, args);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception invoking EX3 entrymethod, msg: {0}", e.Message);
                }
            }
        }

        /// <summary>
        /// Array of arguments for Empiri X3s entrypoint method 
        /// </summary>
        private object[] GetEx3Args(DbInfo dbInfo, string plan_Ruid, Guid sessionGuid, string userId)
        {
            Object[] args = new Object[7];
            args[0] = "Server=" + dbInfo.IpAdress + ";Database=" + dbInfo.Name + ";User ID=" + dbInfo.UserName + ";Password=" + dbInfo.Password + ";";
            args[1] = new System.Globalization.CultureInfo("sv-SE");
            args[2] = userId;
            args[3] = plan_Ruid;
            args[4] = sessionGuid;
            args[5] = "EX3.Client.Form_Main";
            args[6] = null;

            return args;
        }

        /// <summary>
        /// <para></para>
        /// <para></para>
        /// </summary>
        private Type GetEx3Type(Assembly assembly)
        {
            String typeName = "EX3.Client.Program";
            Type type = null;
            try
            {
                type = assembly.GetType(typeName);
            }
            catch (Exception e)
            {
                Console.WriteLine("Assembly get type exception, message: {0}", e.Message);
            }

            return type;
        }

        private SecureString ConvertToSecureString(string dbPassword)
        {
            var secure = new SecureString();
            foreach (char c in dbPassword)
            {
                secure.AppendChar(c);
            }

            return secure;
        }

        private string GetComputerName()
        {
            return System.Environment.MachineName;
        }

        private object GetIpAddress()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                return null;
            }

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            return host
                .AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString();
        }

        private string GetMacAddress()
        {
            return NetworkInterface.GetAllNetworkInterfaces().Where(nic => nic.OperationalStatus == OperationalStatus.Up).Select(nic => nic.GetPhysicalAddress().ToString()).FirstOrDefault();
        }
    }
}
