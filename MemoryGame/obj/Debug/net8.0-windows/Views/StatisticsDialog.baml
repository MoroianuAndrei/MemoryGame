   M S B A M L   `   `   `  ����  
MemoryGame'    !MemoryGame.Views.StatisticsDialog   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Player Statistics��5   	   $	��400��6#   $	��600��60    � WindowStartupLocation$ CenterOwner=�5   	    � 
ResizeMode$ NoResize=�5   	   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5	      $
��Auto �6   �� 5
      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��Player Statistics��6!   $��22(�5      $
��Bold"�6"   $��0,0,0,10q�64   &   System.Windows.Controls.DataGrid  5   
   -   5        $��StatsDataGrid��5       $��1��6      AutoGenerateColumns$ False��5         
IsReadOnly$
 True��5         CanUserAddRows$ False��5         CanUserDeleteRows$ False��5         CanUserReorderColumns$ False��5      	   CanUserResizeRows$	 False��5      
   CanUserSortColumns$

 True��5         HeadersVisibility$ Column=�5         AlternatingRowBackground
 �����$��1q�5      
�������$��0,0,0,10q�5         Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Player Name��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6W   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6:   �� *
Username+6W     5       $ Games Played��6%   $ * 6[    6;   �� *GamesPlayed+6[     5!      $ 	Games Won��6%   $ * 6U    68   �� *
GamesWon+6U     5"      $ Win Rate (%)��6%   $ * 6W    6;   �� *	WinRate+6W     5#      $ Total Score��6%   $ * 6Y    6:   �� *
TotalScore+6Y     5$      $ Total Play Time��6%   $ * 6d    6>   �� *FormattedPlayTime+6d   5&      �� 5(   
   -   5        $��2��5(      $��Close��6   $	��100��6.   $��30��6:   $��Right=�5)      5*      5+      5,      