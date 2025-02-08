using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.PerformanceData;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;


namespace Lb1SysProgr
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            UserControl ListItem = new ListItem();
            populateItems();
        }

        public const string cs = "User ID=postgres;Password=123123;Host=localhost;Port=5432;Database=lb1;";

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public int GetPartnersCount()
        {
            int count = 0;

            try
            {
                using (var connection = new NpgsqlConnection(cs))
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM public.partners;";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        count = Convert.ToInt32(command.ExecuteScalar());
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return count;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show($"Количество партнёров: {GetPartnersCount()}");
        }

        private decimal CountPercent(int partnerId)
        {
            int totalQuantity = 0;
            try
            {
                using (var connection = new NpgsqlConnection(cs))
                {
                    connection.Open();

                    string query = @"SELECT SUM(pp.quantity) FROM public.partner_products pp WHERE pp.partner_id = @partnerId";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@partnerId", partnerId);

                        var result = command.ExecuteScalar();

                        //MessageBox.Show("Total Quantity for Partner ID " + partnerId+1 + ": " + result);

                        if (result != DBNull.Value)
                            totalQuantity += Convert.ToInt32(result);
                    }
                }

                decimal discount = 0;

                if (totalQuantity <= 10000)
                    discount = 0;
                else if (totalQuantity > 10000 && totalQuantity <= 50000)
                    discount = 5;
                else if (totalQuantity > 50000 && totalQuantity <= 300000)
                    discount = 10;
                else
                    discount = 15;
                return discount;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }

        private void populateItems()
        {
            int partnerCount = GetPartnersCount();
            ListItem[] listItems = new ListItem[partnerCount];

            flowLayoutPanel1.Controls.Clear();
            try
            {
                using (var connection = new NpgsqlConnection(cs))
                {
                    connection.Open();

                    string query = "SELECT partner_type, name, director, phone, rating FROM public.partners";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            int index = 0;

                            while (reader.Read() && index < partnerCount)
                            {
                                listItems[index] = new ListItem
                                {
                                    Type = reader.GetString(0),
                                    Name = reader.GetString(1),
                                    Director = reader.GetString(2),
                                    Phone = "+7 " + reader.GetString(3),
                                    Rating = "Рейтинг: " + reader.GetInt32(4)
                                };

                                listItems[index].Percent = CountPercent(index + 1) + "%";

                                flowLayoutPanel1.Controls.Add(listItems[index]);
                                index++;
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
