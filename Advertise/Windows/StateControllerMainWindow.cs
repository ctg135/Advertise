using System;
namespace Advertise.Windows
{
    public class StateControllerMainWindow
    {
        /// <summary>
        /// Главное окно
        /// </summary>
        MainWindow Item;
        /// <summary>
        /// Конструкттор для контроллера состояний
        /// </summary>
        /// <param name="MainWindow">Экземпляр главного окна</param>
        public StateControllerMainWindow(MainWindow MainWindow)
        {
            Item = MainWindow;
            Item.TableSelected += StateControllerMainWindow_TableSelected;
            Item.RowSelected += Item_RowSelected;
            Item.RowsSelected += Item_RowsSelected;
            Item.RowNotSelected += Item_RowNotSelected;
            Item.TableRefreshing += Item_TableRefreshed;
        }
        /// <summary>
        /// Обработчик события оновления таблицы
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void Item_TableRefreshed(object sender, EventArgs e)
        {
            if(Item.TableSelector.SelectedItem.ToString() != string.Empty)
            {
                Item.SetUpTable(Item.TableSelector.SelectedItem.ToString());
            }
            else
            {
                Item_RowNotSelected(this, new EventArgs());
            }
        }
        /// <summary>
        /// Обраблотчик события невыбранных строк
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void Item_RowNotSelected(object sender, EventArgs e)
        {
            Item.ButtonEdit.IsEnabled = false;
            Item.ButtonDelete.IsEnabled = false;
            if (Item.TableSelector.SelectedItem.ToString() == string.Empty) 
                Item.ButtonAdd.IsEnabled = false;
            else 
                Item.ButtonAdd.IsEnabled = true;
        }
        /// <summary>
        /// Обраблотчик события выбранных строк
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void Item_RowsSelected(object sender, EventArgs e)
        {
            StateControllerMainWindow_TableSelected(sender, e);
            Item.ButtonEdit.IsEnabled = false;
            Item.ButtonDelete.IsEnabled = true;
        }
        /// <summary>
        /// Обраблотчик события выбранной строки
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void Item_RowSelected(object sender, EventArgs e)
        {
            StateControllerMainWindow_TableSelected(sender, e);
            Item.ButtonEdit.IsEnabled = true;
            Item.ButtonDelete.IsEnabled = true;
        }
        /// <summary>
        /// Обраблотчик события выбранной таблицы
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события</param>
        private void StateControllerMainWindow_TableSelected(object sender, EventArgs e)
        {
            string selectedTable = Item.TableSelector.SelectedItem.ToString();
            if(selectedTable != string.Empty)
            {
                Item.ButtonExport.IsEnabled = true;
                Item.ButtonAdd.IsEnabled = true;
                Item.ButtonDelete.IsEnabled = false;
                Item.ButtonEdit.IsEnabled = false;
            }
            else
            {
                Item.grid.Items.Clear();
                Item.grid.Columns.Clear();
                Item.ButtonAdd.IsEnabled = false;
                Item.ButtonDelete.IsEnabled = false;
                Item.ButtonEdit.IsEnabled = false;
                Item.ButtonExport.IsEnabled = false;
            }
        }
    }
}
