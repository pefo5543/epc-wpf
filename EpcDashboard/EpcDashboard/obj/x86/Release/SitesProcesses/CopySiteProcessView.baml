   M S B A M L   `   `   `  ���� G  CEpcDashboard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:EpcDashboard.SitesProcessesEpcDashboard.SitesProcesses  i9clr-namespace:EpcDashboard.CommonUserInterface.Converters+EpcDashboard.CommonUserInterface.Converters  q=clr-namespace:EpcDashboard.CommonUserInterface.CustomControls/EpcDashboard.CommonUserInterface.CustomControls  5    /EpcDashboard.SitesProcesses.CopySiteProcessView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     ;local)clr-namespace:EpcDashboard.SitesProcesses     Jconv9clr-namespace:EpcDashboard.CommonUserInterface.Converters     Qcontrol=clr-namespace:EpcDashboard.CommonUserInterface.CustomControls       � 	Resources  W  QEpcDashboard.CommonUserInterface.Converters.NegatableBooleanToVisibilityConverter   NegVisConverter   NegateK   EEpcDashboard.CommonUserInterface.Converters.CopyCustomerNameConverter  CustomerNameConverter��%%   &	        &	         .   .���  � RowDefinitions �� $
��Auto ��� $��* ��� $
��Auto �.+�C   =EpcDashboard.CommonUserInterface.CustomControls.ContentHeader  $��0�� � DataContext �� *ContentHeader+��  �� Orientation$ Vertical=�$��1��.+��  �� $
��Auto ��� $	��420 ��� $
��Auto �.+�� $��0��$��0,10,0,4q� � ColumnDefinitions �� $	��150 ��� $	��150 �.+��� $��0�� �� TextAlignment$ Center=�$��Customer��$
��Bold"��� $��1��$ Center=�$
��Site��$
��Bold"�y��� *IsSite+ �� 	Converter# [  �� -    ��Name$ copyList��	 �� SelectionMode$	 Multiple=�
 � Cursor$

 Hand��$��Disabled=�$��1������ *	TableData+����  �� 
TargetType" ��.���  �� Property �@��
IsSelected �� Value ��  �� Mode$ TwoWay=� �� Path$ 
IsSelected���  � ��
 �������  � ��$ 0,0,0,1q���  � �� �#eef9f0���� .R�� 
 � Tag �� *DataContext+ �� RelativeSource ��  �� Mode$ FindAncestor=� �� AncestorType$ ListBoxL� �� $	��150 ��� $��* �.+��� $��0��$
��Left=���`�  `� 	Converter# [ .1��� $ DataContext.IsSite� �� " ���� $  ��� $��1��$
��Left=����� $
 Name�y��� *DataContext.IsSite+ �� " ��# [  � $��2��$��0,3,0,3q�$��Center=�.+��� $
��Copy��$
��Left=�$��25,5,0,0q�$	��Top=�$��75������ *	TableData+���� $ CopyCommand��� $��Cancel��$
��Left=�$��	150,5,0,0q�$	��Top=�$��75������ *CancelCommand+