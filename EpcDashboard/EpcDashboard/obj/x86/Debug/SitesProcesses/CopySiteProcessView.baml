   M S B A M L   `   `   `  ����G  CEpcDashboard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:EpcDashboard.SitesProcessesEpcDashboard.SitesProcesses  5      i9clr-namespace:EpcDashboard.CommonUserInterface.Converters+EpcDashboard.CommonUserInterface.Converters  5      q=clr-namespace:EpcDashboard.CommonUserInterface.CustomControls/EpcDashboard.CommonUserInterface.CustomControls  5      5    /EpcDashboard.SitesProcesses.CopySiteProcessView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ;local)clr-namespace:EpcDashboard.SitesProcesses     5      Jconv9clr-namespace:EpcDashboard.CommonUserInterface.Converters     5      Qcontrol=clr-namespace:EpcDashboard.CommonUserInterface.CustomControls     5        � 	Resources  5      W  QEpcDashboard.CommonUserInterface.Converters.NegatableBooleanToVisibilityConverter   NegVisConverter   NegateK   EEpcDashboard.CommonUserInterface.Converters.CopyCustomerNameConverter  CustomerNameConverter��%E   &	        &	        5   
    . 5   5     5   
   6)   5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   C   =EpcDashboard.CommonUserInterface.CustomControls.ContentHeader  $��0��5        � DataContext 5       �� *ContentHeader+5       �� 5   
    �� Orientation$ Vertical=�6   $��1��5      .+�5      �  5      �� 5      $
��Auto �6$   �� 5      $	��420 �6$   �� 5       $
��Auto �6$   .+�5#      � $��0��6   $��0,10,0,4q�5$       � ColumnDefinitions 5%      �� 5&      $	��150 �6+   �� 5'      $	��150 �6+   .+�5+      �� $��0��6     �� TextAlignment$ Center=�5,       $��Customer��5-       $
��Bold"�5.       �� 50      $��1��6    $ Center=�51       $
��Site��52       $
��Bold"�53       y�54       �� *IsSite+ �� 	Converter# [  56      �� 58      -   5         ��Name$ copyList��5:      	 �� SelectionMode$	 Multiple=�58      
 � Cursor$

 Hand��5;      $��Disabled=�5<      $��1��5=      ��59      �� *	TableData+��5>      �� 5?       �� 
TargetType" ��6    .�5@      ��  �� Property �@��
IsSelected �� Value 5A   %   ��  �� Mode$ TwoWay=� �� Path$ 
IsSelected��� 5B       � ��
 �����5C   %   �� 5D       � ��$ 0,0,0,1q�5E   %   �� 5F       � �� �#eef9f05G   %   5H      ��5J      �� 5K      .R�5L      � 
 � Tag 6#   �� *DataContext+ �� RelativeSource ��  �� Mode$ FindAncestor=� �� AncestorType$ ListBoxL� 5M   "   �� 5N   &   $	��150 �67   �� 5O   &   $��* �67   .+�5S   "   �� $��0��6(   $
��Left=�5T   (   ��5U   &   `� 5V   *    `� 	Converter# [ 67   .1�5W   .   �� $ DataContext.IsSite�66    5X   6   �� " ��6x   �� 5Y   .   $  �66   6@   5Z   +   5\   #   �� 5^   "   $��1��6(   $
��Left=�5_   (   ��5`   (   �� $
 Name�y�5a   (   �� *DataContext.IsSite+ �� " ��# [  5b      5c      5e      5f      5g      � 5i   
   $��2��6   $��0,3,0,3q�5j      $��Center=�5k      .+�5l      �� $
��Copy��6   $
��Left=�5o      $��25,5,0,0q�5p      $	��Top=�5q      $��75��5r      ��5m      �� *	TableData+��5n      �� $ CopyCommand�5r      �� 5s      $��Cancel��6   $
��Left=�5u      $��	150,5,0,0q�5v      $	��Top=�5w      $��75��5x      ��5t      �� *CancelCommand+5x      5y      5z      5{      