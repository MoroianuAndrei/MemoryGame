   M S B A M L   `   `   `  ����  
MemoryGame(    "MemoryGame.Views.CustomBoardDialog   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Custom Board Size��5   	   $	��200��6#   $	��300��60    � WindowStartupLocation$ CenterOwner=�5   	   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5	      $
��Auto �6   �� 5
      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��0��6!   $��Rows:��61   $��0,0,10,0q�6>   �� 5   
   -   5        $��RowsComboBox��5   0   $��0��6   $��1��6    $	��0,5q�6D    �� SelectedIndex$ 0��5      .@�5      �� $��2��6   �� 5      $��3��6   �� 5      $��4��6   �� 5      $��5��6   �� 5      $��6��6   5      �� 5   
   $��1��6   $��0��6!   $��Columns:��61   $��0,0,10,0q�6A   �� 5   
   -   5        $��ColumnsComboBox��5   0   $��1��6   $��1��6    $	��0,5q�6G   $ 0��5      .@�5       �� $��2��6   �� 5!      $��3��6   �� 5"      $��4��6   �� 5#      $��5��6   �� 5$      $��6��6   5%      �� 5'   
   $��2��6   $��0��6!   $��2��61   $.��(Note: Total number of cells must be even��5(      
�������$��Italic$�5)   &   $	��0,5q�69   �� 5+   
   $��4��6   $��0��6"   $��2��62    �� Orientation$ 
Horizontal=�5,      $��Right=�6.   .+�5-      �� -   5        $��OkButton��5-   "   $��OK��6   $��80��62   $��0,0,10,0q�6=   6O   �� 5.      -   5        $��CancelButton��5.   &   $��Cancel��6   $��80��6:   6E   5/      50      51      