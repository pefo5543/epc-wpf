   M S B A M L   `   `   `  ���� G  CEpcDashboard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullq=clr-namespace:EpcDashboard.CommonUserInterface.CustomControls/EpcDashboard.CommonUserInterface.CustomControls  i9clr-namespace:EpcDashboard.CommonUserInterface.Converters+EpcDashboard.CommonUserInterface.Converters  E    ?EpcDashboard.CommonUserInterface.CustomControls.NavigationLinks   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     Olocal=clr-namespace:EpcDashboard.CommonUserInterface.CustomControls     Jconv9clr-namespace:EpcDashboard.CommonUserInterface.Converters       � 	Resources  W  QEpcDashboard.CommonUserInterface.Converters.NegatableBooleanToVisibilityConverter   VisConverter  NegVisConverter   Negate��%%   &	        &	           . .��� -    �Name$ NavBar��$��2��y��� *DataContext.ShowNavBar+ �� 	Converter# [   �� RelativeSource ��  �� AncestorType" � � ColumnDefinitions �� $
��Auto �.+��� $��8,0,5,0q����� *DataContext.PageViewModels+ �� " ����� .R�A�  A� Columns$ 3������ .R���  ��Name$ navBtn��$��5,10,0,2q�
����������� *
MenuName+���� *DataContext.ChangePageCommand+ �� " ����� y��� *
ShowAsLink+# [    	 �� 
TargetType
 �� Property �� Value��%�   '��       �� "	 ��.��� 
 � �� �#EAF0F0�� 
 � �� �	#FF5E5E9A�� 
 � ��$ 14(��� 
 � ��$ Center=��� 
 � ��$	 160��