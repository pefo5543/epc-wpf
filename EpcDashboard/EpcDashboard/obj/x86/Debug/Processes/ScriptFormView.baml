   M S B A M L   `   `   `  ����G  CEpcDashboard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:EpcDashboard.ProcessesEpcDashboard.Processes  5      q=clr-namespace:EpcDashboard.CommonUserInterface.CustomControls/EpcDashboard.CommonUserInterface.CustomControls  5      +    %EpcDashboard.Processes.ScriptFormView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6local$clr-namespace:EpcDashboard.Processes     5      Qcontrol=clr-namespace:EpcDashboard.CommonUserInterface.CustomControls     5        � 	Resources  5       �� 
TargetType �� Triggers P� Property P� Value �� Property �� Value �� Self �� RelativeSource	 �� Path��%�   '��       �� 5   
   " ��6    5      P� 5       �@=�HasError . . �5      ��  �@�ToolTip 5      �� # Z $2	 ,(Validation.Errors).CurrentItem.ErrorContent�5   O   5      5      5      .��5      � 
 � RowDefinitions
 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   � 5      � 5    
   $��1��6   .+�5!      �� $��
40,10,40,5q�6     5"      ��%\   'J�       �� 5#      " J�6   .�5$      ��  �@��Margin$ 0q�5%   !   5&      5'      5(      5)      � 5*   
   $��2��6   $��
45,5,45,10q�5+      .+�5,      �� $��Execute��6   $
��Left=�5.      $��20,5,0,0q�5/      $	��Top=�50      $��75��51      ��5-      �� *ExecuteCommand+51      �� 52      $��Cancel��6   $��Right=�54      $��0,5,20,0q�55      $	��Top=�56      $��75��57      ��53      �� *CancelCommand+57      58      59      5:      