   M S B A M L   `   `   `  ����G  CEpcDashboard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullq=clr-namespace:EpcDashboard.CommonUserInterface.CustomControls/EpcDashboard.CommonUserInterface.CustomControls  5      q=clr-namespace:EpcDashboard.CommonUserInterface.CustomControls/EpcDashboard.CommonUserInterface.CustomControls  5      i9clr-namespace:EpcDashboard.CommonUserInterface.Converters+EpcDashboard.CommonUserInterface.Converters  5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R8clr-namespace:System.ComponentModel;assembly=WindowsBaseSystem.ComponentModel 5	      L    FEpcDashboard.CommonUserInterface.CustomControls.SiteActionCommandsGrid   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Olocal=clr-namespace:EpcDashboard.CommonUserInterface.CustomControls     5      Qcontrol=clr-namespace:EpcDashboard.CommonUserInterface.CustomControls     5      Jconv9clr-namespace:EpcDashboard.CommonUserInterface.Converters     5      Hscm8clr-namespace:System.ComponentModel;assembly=WindowsBase     5	        � 	Resources  5         	X3Actions �� Source �� SortDescriptions+  %System.ComponentModel.SortDescription   PropertyName  OpenFolderActions  
RDPActions  
VNCActions  
ExeActionsW  QEpcDashboard.CommonUserInterface.Converters.NegatableBooleanToVisibilityConverter ) %NegatableBooleanToVisibilityConverter��%v  &	        &	 l     &	 �     &	 M    &	 �    &	 '    �� 5   
    6   �� *Actions.X3Actions+ 5        5      $ 
ActionName��6&   5      �� 5   
    6   �� *Actions.OpenFolderActions+ 5        5      $ 
ActionName��6&   5      �� 5   
    6   �� *Actions.RDPActions+ 5        5      $ 
ActionName��6&   5!      �� 5#   
    6   �� *Actions.VNCActions+ 5%        5&      $ 
ActionName��6&   5(      �� 5*   
    6   �� *Actions.ExeActions+ 5,        5-      $ 
ActionName��6&   5/        50   
   65   51      .��53      � $��	10,5,10,5q�6   .+�54   
   �� $
��Auto=�6   $��Disabled=�55      .��56      � .+�57      �  � Orientation$ 
Horizontal=�6   .+�59      B   <EpcDashboard.CommonUserInterface.CustomControls.StartEBMSBtn   � DataContext 6+   �� �� 5;      ��6#   ��  �� Source# [   �� Mode$ OneWay=� �� NotifyOnSourceUpdated . 	 �� UpdateSourceTrigger$	 PropertyChanged=���5<      �� 5=      .R�5>   "   � $ 
Horizontal=�6,   5?      ��5A      �� 5B      .R�5C   "   �� 
 ��Name$
 X3��6)     StartX3BtnStyle#��[ 5E   )   $
��Left=�5G   )   $��25��5H   )   $	��110��5I   )   $��4,4,4,4q�5J   )   ��5D   )   �� *
ActionName+$ OneWay=� . $	 PropertyChanged=�
 � Tag 5F   )   �� *DataContext+ �� RelativeSource ��  �� Mode$ FindAncestor=� �� AncestorType$ ItemsControlL� J� InputBindings 5L   &   d� 5N   *    d� Gesture$ 	LeftClicka�67    �� Command 5O   7   �� *%#DataContext.Actions.StartEX3Command+ �� " �� �� CommandParameter 5P   7   ��  � ContextMenu 5S   &   �� 5T   *   .@�5U   .   m� $
��Edit��67    m� Command 5V   7   �� */-PlacementTarget.Tag.Actions.EditActionCommand+ �� $ FindAncestor=�$ ContextMenuL� m� CommandParameter 5W   7   ��  m� Icon 5X   2   �� 5Y   6   $1��+pack://application:,,,/Resources/edit16.png��6<   5[   /   m� 5\   .   $��Delete��67    5]   7   �� *)'PlacementTarget.Tag.DeleteActionCommand+ �� $ FindAncestor=�$ ContextMenuL� 5^   7   ��  5_   2   �� 5`   6   $3��-pack://application:,,,/Resources/delete16.png��6<   5b   /   5c   +   5e   #   5f      5h      �� 5k      ��6#   �� # [ $ OneWay=� . $	 PropertyChanged=���5l      �� 5m      .R�5n   "   � $ 
Horizontal=�6,   5o      ��5q      �� 5r      .R�5s   "   �� $
 
OpenFolder��6)     OpenFolderActionBtnStyle#��[ 5u   )   $
��Left=�5w   )   $��25��5x   )   $	��110��5y   )   $��4,4,4,4q�5z   )   ��5t   )   �� *
ActionName+$ OneWay=� . $	 PropertyChanged=� 5v   )   �� *DataContext+ �� $ FindAncestor=�$ ItemsControlL� 5|   &   d� 5~   *   $ 	LeftClicka�67    5   7   �� *&$DataContext.Actions.MapFolderCommand+ �� " �� 5�   7   ��  5�   &   �� 5�   *   .@�5�   .   m� $
��Edit��67    5�   7   �� */-PlacementTarget.Tag.Actions.EditActionCommand+ �� $ FindAncestor=�$ ContextMenuL� 5�   7   ��  5�   2   �� 5�   6   $1��+pack://application:,,,/Resources/edit16.png��6<   5�   /   m� 5�   .   $��Delete��67    5�   7   �� *)'PlacementTarget.Tag.DeleteActionCommand+ �� $ FindAncestor=�$ ContextMenuL� 5�   7   ��  5�   2   �� 5�   6   $3��-pack://application:,,,/Resources/delete16.png��6<   5�   /   5�   +   5�   #   5�      5�      �� 5�      ��6#   �� # [ $ OneWay=� . $	 PropertyChanged=���5�      �� 5�      .R�5�   "   � $ 
Horizontal=�6,   5�      ��5�      �� 5�      .R�5�   "   �� $	
 RDP��6)     RDPActionBtnStyle#��[ 5�   )   $
��Left=�5�   )   $��25��5�   )   $	��110��5�   )   $��4,4,4,4q�5�   )   ��5�   )   �� *
ActionName+$ OneWay=� . $	 PropertyChanged=� 5�   )   �� *DataContext+ �� $ FindAncestor=�$ ItemsControlL� 5�   &   d� 5�   *   $ 	LeftClicka�67    5�   7   �� *#!DataContext.Actions.RunRDPCommand+ �� " �� 5�   7   ��  5�   &   �� 5�   *   .@�5�   .   m� $
��Edit��67    5�   7   �� */-PlacementTarget.Tag.Actions.EditActionCommand+ �� $ FindAncestor=�$ ContextMenuL� 5�   7   ��  5�   2   �� 5�   6   $1��+pack://application:,,,/Resources/edit16.png��6<   5�   /   m� 5�   .   $��Delete��67    5�   7   �� *)'PlacementTarget.Tag.DeleteActionCommand+ �� $ FindAncestor=�$ ContextMenuL� 5�   7   ��  5�   2   �� 5�   6   $3��-pack://application:,,,/Resources/delete16.png��6<   5�   /   5�   +   5�   #   5�      5�      �� 5�      ��6#   �� # [ $ OneWay=� . $	 PropertyChanged=���5�      �� 5�      .R�5�   "   � $ 
Horizontal=�6,   5�      ��5�      �� 5�      .R�5�   "   �� $	
 VNC��6)    	 VNCActionBtnStyle#��[	 5�   )   $
��Left=�5�   )   $��25��5�   )   $	��110��5�   )   $��4,4,4,4q�5�   )   ��5�   )   �� *
ActionName+$ OneWay=� . $	 PropertyChanged=� 5�   )   �� *DataContext+ �� $ FindAncestor=�$ ItemsControlL� 5�   &   d� 5�   *   $ 	LeftClicka�67    5�   7   �� *#!DataContext.Actions.RunVNCCommand+ �� " �� 5�   7   ��  5�   &   �� 5�   *   .@�5�   .   m� $
��Edit��67    5�   7   �� */-PlacementTarget.Tag.Actions.EditActionCommand+ �� $ FindAncestor=�$ ContextMenuL� 5�   7   ��  5�   2   �� 5�   6   $1��+pack://application:,,,/Resources/edit16.png��6<   5�   /   m� 5�   .   $��Delete��67    5�   7   �� *)'PlacementTarget.Tag.DeleteActionCommand+ �� $ FindAncestor=�$ ContextMenuL� 5�   7   ��  5�   2   �� 5�   6   $3��-pack://application:,,,/Resources/delete16.png��6<   5�   /   5�   +   5�   #   5�      5�      �� 5�      ��6#   �� # [ $ OneWay=� . $	 PropertyChanged=���5�      �� 5�      .R�5�   "   � $ 
Horizontal=�6,   5�      ��5     �� 5     .R�5  "   �� $	
 Exe��6)    
 ExeActionBtnStyle#��[
 5  )   $
��Left=�5  )   $��25��5  )   $	��110��5	  )   $��4,4,4,4q�5
  )   ��5  )   �� *
ActionName+$ OneWay=� . $	 PropertyChanged=� 5  )   �� *DataContext+ �� $ FindAncestor=�$ ItemsControlL� 5  &   d� 5  *   $ 	LeftClicka�67    5  7   �� *#!DataContext.Actions.RunExeCommand+ �� " �� 5  7   ��  5  &   �� 5  *   .@�5  .   m� $
��Edit��67    5  7   �� */-PlacementTarget.Tag.Actions.EditActionCommand+ �� $ FindAncestor=�$ ContextMenuL� 5  7   ��  5  2   �� 5  6   $1��+pack://application:,,,/Resources/edit16.png��6<   5  /   m� 5  .   $��Delete��67    5  7   �� *)'PlacementTarget.Tag.DeleteActionCommand+ �� $ FindAncestor=�$ ContextMenuL� 5  7   ��  5  2   �� 5   6   $3��-pack://application:,,,/Resources/delete16.png��6<   5"  /   5#  +   5%  #   5&     5(     5)     5*     5+     5,     5-     