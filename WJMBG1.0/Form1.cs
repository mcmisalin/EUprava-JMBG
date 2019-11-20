using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WJMBG1._0
{
    public partial class Form1 : Form
    {
        const int MAX_VALID_YR = 9999;
        const int MIN_VALID_YR = 1900; 

        public Form1()
        {
            InitializeComponent();
            Application.Idle += new EventHandler(Application_Idle);
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            //Onemogucavanje unosenja u text i check boxove rezultata
            cbxKBR.AutoCheck = false;
            txtDRR.ReadOnly = true;
            txtImeR.ReadOnly = true;
            txtJBR.ReadOnly = true;
            txtPrezimeR.ReadOnly = true;
            txtPolR.ReadOnly = true;

            txtMRR.ReadOnly = true;
            txtMRR.Multiline = true;
            txtMRR.ScrollBars = ScrollBars.Vertical;
            txtMRR.WordWrap = true;
            

            //Pretrazivanje je moguce samo ukoliko su sva polja popunjena i JMBG ima 13 karaktera
            if (txtIme.Text != "" && txtPrezime.Text != "" && txtJMBG.Text != "")
            {
                if (txtJMBG.Text.Length != 13)
                    btnPretrazi.Enabled = false;
                else
                    btnPretrazi.Enabled = true;
            }
            else
                btnPretrazi.Enabled = false;
               
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

            
            //Pretvaranje JMBG string u niz karaktera
            char[] substrings = txtJMBG.Text.ToCharArray();

            //Konstruisanje pojedinacnih straingova radi lakse provere
            string dan = new string(substrings, 0, 2);
            string mesec = new string(substrings, 2, 2);
            string godina = new string(substrings, 4, 3);

            string regija = new string(substrings, 7, 2);
            char regijaStart = regija[0];

            string JB = new string (substrings, 9,3);
            string KB = new string (substrings, substrings.Length-1, 1);

            //poziva se metoda za proveru validnosti datuma
            if (isValidDate(Int32.Parse(dan), Int32.Parse(mesec), Int32.Parse(godina)))
            {
                //Provera regije
                switch (regijaStart)
                {
                    case '0':
                        proveraRegijeStranac(regija);
                        break;
                    case '1':
                        proveraRegijeBiH(regija);
                        break;
                    case '2':
                        proveraRegijeCG(regija);
                        break;
                    case '3':
                        proveraRegijeHR(regija);
                        break;
                    case '4':
                        proveraRegijeMCD(regija);
                        break;
                    case '5':
                        proveraRegijeSLO(regija);
                        break;
                    case '7':
                        proveraRegijeCentralnaSrbija(regija);
                        break;
                    case '8':
                        proveraRegijeAPVojvodina(regija);
                        break;
                    case '9':
                        proveraRegijeAPKosovo(regija);
                        break;
                    default:
                        alertMessage();
                        break;

                }
                //provera jedinstvenog broja
                if (Enumerable.Range(0, 499).Contains(Int32.Parse(JB)))
                {
                    txtJBR.Text = JB;
                    txtPolR.Text = "muski";
                }
                else if (Enumerable.Range(500, 999).Contains(Int32.Parse(JB)))
                {
                    txtJBR.Text = JB;
                    txtPolR.Text = "zenski";
                }
                else
                {
                    alertMessage();
                }

                //Provera kontrolnog broja
                int kontrolni =11 - ((7 * (Int32.Parse(dan[0].ToString()) + Int32.Parse(godina[2].ToString())) + 6 * (Int32.Parse(dan[1].ToString()) + Int32.Parse(regija[0].ToString()))
                                        + 5 * (Int32.Parse(mesec[0].ToString()) + Int32.Parse(regija[1].ToString())) + 4 * (Int32.Parse(mesec[1].ToString()) + Int32.Parse(JB[0].ToString()))
                                        + 3 * (Int32.Parse(godina[0].ToString()) + Int32.Parse(JB[1].ToString())) + 2 * (Int32.Parse(godina[1].ToString()) + Int32.Parse(JB[2].ToString()))) % 11);

                if (Int32.Parse(KB) == kontrolni){
                    cbxKBR.Checked = true;
                    txtImeR.Text = txtIme.Text;
                    txtPrezimeR.Text = txtPrezime.Text;
                if (godina.StartsWith("9"))
                    txtDRR.Text = dan + "." + mesec + "." + "1" + godina + ".";
                else
                    txtDRR.Text = dan + "." + mesec + "." + "2" + godina + ".";
                }
                    
                else
                    alertMessage();

                
            }
            else
            {
                alertMessage();
            }

           }
            catch (FormatException)
            {
                alertMessage();
            }


        }

        private void proveraRegijeAPKosovo(string regija)
        {
            if (regija.Equals("91"))
                txtMRR.Text = "Priština region";
            if (regija.Equals("92"))
                txtMRR.Text = "Kosovska Mitrovica region";
            if (regija.Equals("93"))
                txtMRR.Text = "Peć region";
            if (regija.Equals("94"))
                txtMRR.Text = "Đakovica region";
            if (regija.Equals("95"))
                txtMRR.Text = "Prizren region";
            if (regija.Equals("96"))
                txtMRR.Text = "Kosovsko Pomoravski okrug";
        }

        private void proveraRegijeAPVojvodina(string regija)
        {
            if (regija.Equals("80"))
                txtMRR.Text = "Novi Sad region";
            if (regija.Equals("81"))
                txtMRR.Text = "Sombor region";
            if (regija.Equals("82"))
                txtMRR.Text = "Subotica region";
            if (regija.Equals("85"))
                txtMRR.Text = "Zrenjanin region";
            if (regija.Equals("86"))
                txtMRR.Text = "Pančevo region";
            if (regija.Equals("87"))
                txtMRR.Text = "Kikinda region";
            if (regija.Equals("88"))
                txtMRR.Text = "Ruma region";
            if (regija.Equals("89"))
                txtMRR.Text = "Sremska Mitrovica region";
        }

        private void proveraRegijeCentralnaSrbija(string regija)
        {
            if (regija.Equals("71"))
                txtMRR.Text = "Beograd region";
            if (regija.Equals("72"))
                txtMRR.Text = "Šumadija";
            if (regija.Equals("73"))
                txtMRR.Text = "Niš region";
            if (regija.Equals("74"))
                txtMRR.Text = "Južna Morava";
            if (regija.Equals("75"))
                txtMRR.Text = "Zaječar";
            if (regija.Equals("76"))
                txtMRR.Text = "Podunavlje";
            if (regija.Equals("77"))
                txtMRR.Text = "Podrinje i Kolubara";
            if (regija.Equals("78"))
                txtMRR.Text = "Kraljevo region";
            if (regija.Equals("79"))
                txtMRR.Text = "Užice region";
        }

        private void proveraRegijeSLO(string regija)
        {
            txtMRR.Text = "Slovenija";
        }

        private void proveraRegijeMCD(string regija)
        {
            if (regija.Equals("41"))
                txtMRR.Text = "Bitola";
            if (regija.Equals("42"))
                txtMRR.Text = "Kumanovo";
            if (regija.Equals("43"))
                txtMRR.Text = "Ohrid";
            if (regija.Equals("44"))
                txtMRR.Text = "Prilep";
            if (regija.Equals("45"))
                txtMRR.Text = "Skopje";
            if (regija.Equals("46"))
                txtMRR.Text = "Strumica";
            if (regija.Equals("47"))
                txtMRR.Text = "Tetovo";
            if (regija.Equals("48"))
                txtMRR.Text = "Veles";
            if (regija.Equals("49"))
                txtMRR.Text = "Štip";
        }

        private void proveraRegijeHR(string regija)
        {
            if (regija.Equals("30"))
                txtMRR.Text = "Osijek, Slavonija region";
            if (regija.Equals("31"))
                txtMRR.Text = "Bjelovar, Virovitica, Koprivnica, Pakrac, Podravina region";
            if (regija.Equals("32"))
                txtMRR.Text = "Varaždin, Međimurje region";
            if (regija.Equals("33"))
                txtMRR.Text = "Zagreb";
            if (regija.Equals("34"))
                txtMRR.Text = "Karlovac";
            if (regija.Equals("35"))
                txtMRR.Text = "Gospić, Lika region";
            if (regija.Equals("36"))
                txtMRR.Text = "Rijeka, Pula, Istra and Primorje region";
            if (regija.Equals("37"))
                txtMRR.Text = "Sisak, Banovina region";
            if (regija.Equals("38"))
                txtMRR.Text = "Split, Zadar, Dubrovnik, Dalmacija region";
            if (regija.Equals("39"))
                txtMRR.Text = "ostalo";
        }

        private void proveraRegijeCG(string regija)
        {
            
            if (regija.Equals("21"))
                txtMRR.Text = "Podgorica";
            if (regija.Equals("26"))
                txtMRR.Text = "Nikšić";
            if (regija.Equals("29"))
                txtMRR.Text = "Pljevlja";
        }

        private void proveraRegijeBiH(string regija)
        {
            if (regija.Equals("10"))
                txtMRR.Text = "Banja Luka";
            if (regija.Equals("11"))
                txtMRR.Text = "Bihać";
            if (regija.Equals("12"))
                txtMRR.Text = "Doboj";
            if (regija.Equals("13"))
                txtMRR.Text = "Goražde";
            if (regija.Equals("14"))
                txtMRR.Text = "Livno";
            if (regija.Equals("15"))
                txtMRR.Text = "Mostar";
            if (regija.Equals("16"))
                txtMRR.Text = "Prijedor";
            if (regija.Equals("17"))
                txtMRR.Text = "Sarajevo";
            if (regija.Equals("18"))
                txtMRR.Text = "Tuzla";
            if (regija.Equals("19"))
                txtMRR.Text = "Zenica";
        }

        private void proveraRegijeStranac(string regija)
        {
            if (regija.Equals("01"))
                txtMRR.Text = "stranci u BiH";
            if (regija.Equals("02"))
                txtMRR.Text = "stranci u Crnoj Gori";
            if (regija.Equals("03"))
                txtMRR.Text = "stranci u Hrvatskoj";
            if (regija.Equals("04"))
                txtMRR.Text = "stranci u Makedoniji";
            if (regija.Equals("05"))
                txtMRR.Text = "stranci u Sloveniji";
            if (regija.Equals("06"))
                txtMRR.Text = "stranci u Srbiji (bez pokrajina)";
            if (regija.Equals("07"))
                txtMRR.Text = "stranci u Vojvodini";
            if (regija.Equals("08"))
                txtMRR.Text = "stranci na Kosovu i Metohiji";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        } 

        public void alertMessage()
        {
            DialogResult result = MessageBox.Show("JMBG je neispravan! Zelite li da nastavite?",
                "Error",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
                Application.Exit();

        }

        bool isLeap(int year)
        {
            return (((year % 4 == 0) &&
                     (year % 100 != 0)) ||
                     (year % 400 == 0));
        }

        
        bool isValidDate(int d, int m, int y)
        {
            if (y.ToString().StartsWith("9"))
                y = Int32.Parse("1" + y.ToString());
            else
                y = Int32.Parse("2" + y.ToString());

            if (y > MAX_VALID_YR ||
                 y < MIN_VALID_YR)
                return false;

            if (m < 1 || m > 12)
                return false;
            if (d < 1 || d > 31)
                return false;

            // Handle February month  
            // with leap year 
            if (m == 2)
            {
                if (isLeap(y))
                    return (d <= 29);
                else
                    return (d <= 28);
            }

            // Months of April, June,  
            // Sept and Nov must have  
            // number of days less than 
            // or equal to 30. 
            if (m == 4 || m == 6 ||
                m == 9 || m == 11)
                return (d <= 30);

            return true;
        } 
    }
}
