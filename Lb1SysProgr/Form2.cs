using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lb1SysProgr
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public const string cs = "User ID=postgres;Password=123123;Host=localhost;Port=5432;Database=lb1;";

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void LoadPartners()
        {
            comboBox2.Items.Clear();

            comboBox2.Items.Add("!!!ДОБАВИТЬ НОВОГО!!!");

            try
            {
                using (var connection = new NpgsqlConnection(cs))
                {
                    connection.Open();

                    string query = "SELECT name FROM public.partners";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                comboBox2.Items.Add(reader.GetString(0));
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных партнёров: " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(new string[] { "ООО", "ОАО", "ЗАО", "ПАО" });
            LoadPartners();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem == null) return;

            if (comboBox2.SelectedItem.ToString() == "!!!ДОБАВИТЬ НОВОГО!!!")
            {
                textBox1.Clear();
                comboBox1.SelectedItem = -1;
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
            };

            try
            {
                using (var connection = new NpgsqlConnection(cs))
                {
                    connection.Open();

                    string query = "SELECT name, partner_type, rating, address, director, phone, email FROM public.partners WHERE name = @name";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", comboBox2.SelectedItem.ToString());

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                textBox1.Text = reader.GetString(0);
                                comboBox1.Text = reader.GetString(1);
                                textBox2.Text = Convert.ToString(reader.GetInt32(2));
                                textBox3.Text = reader.GetString(3);
                                textBox4.Text = reader.GetString(4);
                                textBox5.Text = "+7 " + reader.GetString(5);
                                textBox6.Text = reader.GetString(6);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных о партнёре: " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem == null || comboBox2.SelectedItem.ToString() == "!!!ДОБАВИТЬ НОВОГО!!!")
            {
                if(Convert.ToInt32(textBox2.Text) < 0)
                {
                    MessageBox.Show("Рейтинг не может быть отрицательным числом!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                try
                {
                    using (var connection = new NpgsqlConnection(cs))
                    {
                        connection.Open();

                        string query = "INSERT INTO public.partners (name, partner_type, rating, address, director, phone, email) " +
                            "VALUES (@name, @type, @rating, @address, @director, @phone, @email)";

                        using (var command = new NpgsqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@name", textBox1.Text);
                            command.Parameters.AddWithValue("@type", comboBox1.SelectedItem.ToString());
                            command.Parameters.AddWithValue("@rating", Convert.ToInt32(textBox2.Text));
                            command.Parameters.AddWithValue("@address", textBox3.Text);
                            command.Parameters.AddWithValue("@director", textBox4.Text);
                            command.Parameters.AddWithValue("@phone", textBox5.Text.Replace("+7 ", ""));
                            command.Parameters.AddWithValue("@email", textBox6.Text);

                            command.ExecuteNonQuery();
                        }

                        connection.Close();
                    }

                    LoadPartners();

                    MessageBox.Show("Новый партнёр добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка добавления нового партнёра: " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } 
            else
            {
                try
                {
                    using (var connection = new NpgsqlConnection(cs))
                    {
                        connection.Open();

                        string query = "UPDATE public.partners  SET name = @name, partner_type = @type, rating = @rating, address = @address, director = @director, phone = @phone, email = @email WHERE name = @namePartn";

                        using (var command = new NpgsqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@namePartn", comboBox2.SelectedItem.ToString());

                            command.Parameters.AddWithValue("@name", textBox1.Text);
                            command.Parameters.AddWithValue("@type", comboBox1.SelectedItem.ToString());
                            command.Parameters.AddWithValue("@rating", Convert.ToInt32(textBox2.Text));
                            command.Parameters.AddWithValue("@address", textBox3.Text);
                            command.Parameters.AddWithValue("@director", textBox4.Text);
                            command.Parameters.AddWithValue("@phone", textBox5.Text.Replace("+7 ", ""));
                            command.Parameters.AddWithValue("@email", textBox6.Text);

                            int a = command.ExecuteNonQuery();

                            if (a > 0) MessageBox.Show("Данные парнёра обновлены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            else MessageBox.Show("Парнёр не найден!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        connection.Close();
                    }

                    LoadPartners();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
