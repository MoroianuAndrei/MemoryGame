   M S B A M L   `   `   `  ����E  AMemoryGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:MemoryGame.ViewModelsMemoryGame.ViewModels  5      %    MemoryGame.Views.CreateUserView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      2vm#clr-namespace:MemoryGame.ViewModels     5      $	��500��5      $	��700��6   .��5      �   � RowDefinitions  5   
   �� 5      $
��Auto �6   �� 5	      $��* �6   �� 5
      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��Create New User��6   $��Center=�5      $��16(�5      $��	0,10,0,10q�5      �� 5   
   $��1��6   $��Center=�5      $��Center=�5      $	��150��5      $	��150��5      ��5      �� *CurrentImage+5      �� 5   
   $��2��6    �� Orientation$ 
Horizontal=�5      $��Center=�5      $��	0,10,0,10q�5       .+�5!      �� $��<<��6   $��5q�6L   $��50��6W   ��6(   �� *PreviousCommand+6W   �� 5"      $��>>��6   $��5q�6H   $��50��6S   ��6(   �� *NextCommand+6S   5#      � 5&   
   $��3��6   $��
10,0,10,10q�6     5'      �� 5(      $
��Auto �6    �� 5)      $
��Auto �6    .+�5,      � $��0��6    � ColumnDefinitions 5-      �� 5.      $
��Auto �6'   �� 5/      $��* �6'   .+�52      �� $��	Username:��6   $��0��53      $��Center=�54      $��0,0,10,0q�55      �� 56      $��1��6   $��Center=�58      ��57      �� *
Username+ �� UpdateSourceTrigger$ PropertyChanged=�58      59      �� 5;      $��1��6   $ 
Horizontal=�5<      $��Right=�5=      $��0,10,0,0q�5>      .+�5?      �� $��Cancel��6   $��5q�5A      $��70��5B      ��5@      �� *CancelCommand+5B      �� 5C      $��OK��6   $��5q�5E      $��70��5F      ��5D      �� *SaveCommand+5F      5G      5H      5I      5J      