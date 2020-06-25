using System;
using System.Data.Linq;
using System.Windows;
using System.Linq;
using System.Windows.Controls;

namespace LightYagami
{
    public partial class MainWindow : Window
    {
         int  key = DateTime.Now.Millisecond;
        readonly DataContext db = new DataContext(@"Data Source=.\SQLEXPRESS;Initial Catalog=Cifers;Integrated Security=True");
        const int SAVE_ON_BASE = 1, USER_SAVE = 0;string txtForBase;   Random rnd = new Random(); 
        public MainWindow()
        {  
            InitializeComponent(); 
        } 
        private void Encrypt(object sender, RoutedEventArgs e)//шифрует текст
        {  
            if(key==USER_SAVE||key>600)
            {
                key = DateTime.Now.Year%11;
            }
            if (CryptText.Text.Length == 0)
            {
                MessageBox.Show("текст не найден");   
            }
            DecryptText.Text = string.Empty;
            string mainText = CryptText.Text;
            char[] symbols = mainText.ToCharArray();
            string s = "", symb = "ABCK0ячсмитьбю.12LMNOйцукенгшщзхъhirst;xyzPYZ6-9=";
            string q= "", symbB = "abcdefguQRфывапролджэST78klmDEFGHIJnjopq345UVWXvw";
            for (int z = 0; z < key; z++)
            {
               s += symb[rnd.Next(0, symb.Length)];
                q+= symb[rnd.Next(0, symbB.Length)];
            }int x = 0;
            foreach (char i in symbols)
            {
                x++;
                if (DecryptText.Text.Length  % i==0||x%3==0||i=='a' || i == ';' || i == 'l' || i == 'k' || i == 'j' || i == 'h' || i == 'g' || i == 'f' || i == 'd' || i == 's' || i == '\'')
                {
                    DecryptText.Text += i + s;
                }
                else
                {
                  DecryptText.Text +=i+q;
                }
            }  txtForBase = DecryptText.Text;
        } 
        private void Decrypt(TextBox text,TextBox placeForResult)//дешифрует текст
        {
            placeForResult.Text = string.Empty;
            string txt = text.Text;int countSymbolsOnMyText = txt.Length;
            char[] symbol = txt.ToCharArray();
            for(int i = 0; i < countSymbolsOnMyText; i += key+SAVE_ON_BASE)
            {
                placeForResult.Text += symbol[i];
            }
        }
        private void Decrypt_Click(object sender, RoutedEventArgs e)//кнопка дешифрования
        {
            if (DecryptText.Text.Length==0)
            {
                MessageBox.Show("текст не найден");
            }
            Decrypt(DecryptText,CryptText);
        }
        private void SaveDecryptText(object sender,RoutedEventArgs e)//сохранение зашифрованного текста
        {
            if (MessageBox.Show("сохранить ключ для дешифрования в базе?\r\n" +
                      "да-ключ сохранится в базе" +
                      "\r\nнет-вывести на экран,пользователь запоминает ключ",
                      "выбор местоположения ключа", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
             CiferText userCciferText = new CiferText { Key = key, Text = txtForBase,SavePlaceState=SAVE_ON_BASE};
             db.GetTable<CiferText>().InsertOnSubmit(userCciferText);
             db.SubmitChanges();
             MessageBox.Show("Данные успешно сохранены");
            }
            else
            {
                CiferText userCciferText = new CiferText { Key =USER_SAVE, Text = txtForBase, SavePlaceState = USER_SAVE };
                db.GetTable<CiferText>().InsertOnSubmit(userCciferText);
                db.SubmitChanges();
                MessageBox.Show("ваш ключ: "+key);
                MessageBox.Show("Данные успешно сохранены.\r\nне забудьте ключ!");
            }
        }   
        private void NewNote(object sender, RoutedEventArgs e)
        {
            fromDb.Visibility = Visibility.Collapsed;
            afterChoiseBlock.Visibility = Visibility.Visible;
        }
        private void GetList()//выводит список номеров текстов
        {
            fromDb.Visibility = Visibility.Visible;
            dbTexts.Text = string.Empty;
            Table<CiferText> ciferCurrent = db.GetTable<CiferText>();  
            foreach(var b in ciferCurrent)
            {
                dbTexts.Text += "№ " + b.Id + "\r\n";
            }
        }
        private void GetDataFromDB(object sender, RoutedEventArgs e)//кнопка для вывода списка номеров зашифрованных текстов
        {
            afterChoiseBlock.Visibility = Visibility.Collapsed;
            dbTexts.Visibility = Visibility.Visible;
            GetList();
        } 
        private void GetTextById(int id)//вывод на экран дешифрованный текст выбранный пользователем
        {
            dbTexts.Text = string.Empty;
            var txtById = from c in db.GetTable<CiferText>() where c.Id == id  select c;

        foreach (var b in txtById)
        {
            dbTexts.Text += b.Text;
        }
        } 
        private void GetTextById(object sender, RoutedEventArgs e)
        {
            dbTexts.Text = string.Empty;
            if (userChoiceIdOrKey.Text.Length == 0)
            {
                MessageBox.Show("введите номер текста");
            }
            else
            {
            int usId = Convert.ToInt32(userChoiceIdOrKey.Text);
            GetTextById(usId);
            }
        }
    }
}