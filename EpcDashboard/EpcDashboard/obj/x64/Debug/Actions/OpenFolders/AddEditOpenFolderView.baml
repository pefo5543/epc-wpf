   M S B A M L   `   `   `  ����G  CEpcDashboard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:EpcDashboard.ActionsEpcDashboard.Actions  5      i9clr-namespace:EpcDashboard.CommonUserInterface.Converters+EpcDashboard.CommonUserInterface.Converters  5      q=clr-namespace:EpcDashboard.CommonUserInterface.CustomControls/EpcDashboard.CommonUserInterface.CustomControls  5      <    6EpcDashboard.Actions.OpenFolders.AddEditOpenFolderView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4local"clr-namespace:EpcDashboard.Actions     5      Jconv9clr-namespace:EpcDashboard.CommonUserInterface.Converters     5      Rcontrols=clr-namespace:EpcDashboard.CommonUserInterface.CustomControls     5        � 	Resources  5      W  QEpcDashboard.CommonUserInterface.Converters.NegatableBooleanToVisibilityConverter   VisConverter  NegVisConverter   Negate �� 
TargetType �� Triggers P� Property P� Value �� Property �� Value �� Self	 �� RelativeSource
 �� Path��%"  &	        &	      '�� 1       5   
   65     5   
    . 5   5   �� 5   
   " ��6    5      P� 5       �@=�HasError . . �5      ��  �@�ToolTip 5      �� #	 Z $2
 ,(Validation.Errors).CurrentItem.ErrorContent�5   P   5      5      5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5    
   C   =EpcDashboard.CommonUserInterface.CustomControls.ContentHeader  $��0��6G    � DataContext 6!   �� *ContentHeader+6G   � 5!   
   -   5         �Name$ grid1��5!      $	��Top=�6   $��Center=�66   $��	14,13,9,0q�6S   $��1��5"   4    6   �� *FolderAction+ 5#      �� 5$      $
��Auto �6    �� 5%      $
��Auto �6    �� 5&      $
��Auto �6    �� 5'      $
��Auto �6     � ColumnDefinitions 5)      �� 5*      $
��Auto �6#   $��88��60   �� 5+      $
��Auto �6#   $��195.2��60   .+�5-      �� $��Center=�6   $��0��6/   $��Right=�6<   $��0��6X   $��Action Name:��6h   $��25��6   $��
0,0,0.6,10q�6�   �� 5.      -   5         ��Name$ actionNameTextBox��5.      $��Center=�61   $��0��50      $��23��6$   $
��Left=�60   $��1��6K   $	��190��6[   $��
0.4,1,0,10q�6g   ��5/      �� *
ActionName+ �� Mode$ TwoWay=�" �� ValidatesOnNotifyDataErrors .  �� UpdateSourceTrigger$ PropertyChanged=�50   g   �� 52      $��Center=�6   $��1��6/   $��Right=�6<   $��0��6X   $��
Ip Adress:��6h   $��25��6}   $��
0,0,0.6,10q�6�   �� 53      -   5        $ 	ipTextBox��53      $��Center=�6)   $��1��55      $��23��6$   $
��Left=�60   $��1��6K   $	��190��6[   $��
0.4,1,0,10q�6g   ��54      �� *
IpAdress+$ TwoWay=� . $ PropertyChanged=�55   g   �� 57      $��Center=�6   $��2��6/   $��Right=�6<   $��0��6X   $��Server Username:��6h   $��25��6�   $��
0,0,0.6,10q�6�   �� 58      -   5        $ servUsrNameTextBox��58      $��Center=�62   $��2��5:      $��23��6$   $
��Left=�60   $��1��6K   $	��190��6[   $��
0.4,1,0,10q�6g   ��59      �� *ServerUserName+$ TwoWay=� . $ PropertyChanged=�5:   g   �� 5<      $��Center=�6   $��3��6/   $��Right=�6<   $��0��6X   $��Server Password:��6h   $��25��6�   $��
0,0,0.6,10q�6�   �� 5=      -   5        $ servPasswordTextBox��5=      $��Center=�63   $��3��5?      $��23��6$   $
��Left=�60   $��1��6K   $	��190��6[   $��
0.4,1,0,10q�6g   ��5>      �� *ServerPassword+$ TwoWay=� . $ PropertyChanged=�5?   g   5A      � 5B   
   $��2��6   $��45,7.4,45,-7.2q�6   $��Center=�64   .+�5C      �� $
��Save��6   $
��Left=�5F      $��25,5,0,0q�5G      $	��Top=�5H      $��75��5I      ��5D      �� *SaveCommand+y�5E      �� *
EditMode+ �� 	Converter# [  5I      �� 5J      $	��Add��6   $
��Left=�5M      $��25,5,0,0q�5N      $	��Top=�5O      $��75��5P      ��5K      �� *SaveCommand+y�5L      �� *
EditMode+# [ 5P      �� 5Q      $��Cancel��6   $
��Left=�5S      $��	150,5,0,0q�5T      $	��Top=�5U      $��75��5V      ��5R      �� *CancelCommand+5V      5W      5X      5Y      