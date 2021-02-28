using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace kr_v2
{   
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection = null;
        private SqlCommandBuilder sqlBuilder = null;
        private SqlDataAdapter sqlDataAdapter = null;
        private DataSet dataSet = null;
        private bool newRowAdding = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            try
            {
                sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Command] FROM Goods", sqlConnection);

                sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);

                sqlBuilder.GetInsertCommand();
                sqlBuilder.GetDeleteCommand();
                sqlBuilder.GetUpdateCommand();

                dataSet = new DataSet();

                sqlDataAdapter.Fill(dataSet, "Goods");
                dataGridView1.DataSource = dataSet.Tables["Goods"];
                for (int i = 0; i<dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[6, i] = linkCell;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReloadData()
        {
            try
            {
                dataSet.Tables["Goods"].Clear();
                sqlDataAdapter.Fill(dataSet, "Goods");
                dataGridView1.DataSource = dataSet.Tables["Goods"];
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[6, i] = linkCell;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\clouds\OneDrive1\OneDrive - Asian Answers\учеба\2 курс\визуальное программирование и человеко-машинное взаимодействие\кр1\kr-v2\Database1.mdf;Integrated Security=True");
            sqlConnection.Open();

            LoadData();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
             try
            {
                if (e.ColumnIndex==6)
                {
                    string task = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
                    if (task == "Delete")
                    {
                        if (MessageBox.Show("Удалить эту строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            int rowIndex = e.RowIndex;
                            dataGridView1.Rows.RemoveAt(rowIndex);
                            dataSet.Tables["Goods"].Rows[rowIndex].Delete();
                            sqlDataAdapter.Update(dataSet,"Goods");
                        }
                    }
                    else if (task == "Insert") 
                    {
                        int rowIndex = dataGridView1.Rows.Count - 2;

                        DataRow row = dataSet.Tables["Goods"].NewRow();

                        row["Name"] = dataGridView1.Rows[rowIndex].Cells["Name"].Value;
                        row["Category"] = dataGridView1.Rows[rowIndex].Cells["Category"].Value;
                        row["Income"] = dataGridView1.Rows[rowIndex].Cells["Income"].Value;
                        row["Sold"] = dataGridView1.Rows[rowIndex].Cells["Sold"].Value;
                        row["Left"] = dataGridView1.Rows[rowIndex].Cells["Left"].Value;

                        dataSet.Tables["Goods"].Rows.Add(row);
                        dataSet.Tables["Goods"].Rows.RemoveAt(dataSet.Tables["Goods"].Rows.Count - 1);
                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);
                        dataGridView1.Rows[e.RowIndex].Cells[6].Value = "Delete";

                        sqlDataAdapter.Update(dataSet, "Goods");

                        newRowAdding = false;
                    }
                    else if (task == "Update")
                    {
                        int r = e.RowIndex;

                        dataSet.Tables["Goods"].Rows[r]["Name"] = dataGridView1.Rows[r].Cells["Name"].Value;
                        dataSet.Tables["Goods"].Rows[r]["Surname"] = dataGridView1.Rows[r].Cells["Surname"].Value;
                        dataSet.Tables["Goods"].Rows[r]["Age"] = dataGridView1.Rows[r].Cells["Age"].Value;
                        dataSet.Tables["Goods"].Rows[r]["Email"] = dataGridView1.Rows[r].Cells["Email"].Value;
                        dataSet.Tables["Goods"].Rows[r]["Phone"] = dataGridView1.Rows[r].Cells["Phone"].Value;

                        sqlDataAdapter.Update(dataSet, "Goods");

                        dataGridView1.Rows[e.RowIndex].Cells[6].Value = "Delete";
                    }

                    
                }
                ReloadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    newRowAdding = true;
                    int lastRow = dataGridView1.Rows.Count - 2;
                    DataGridViewRow row = dataGridView1.Rows[lastRow];
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[6, lastRow] = linkCell;
                    row.Cells["Command"].Value = "Insert";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (newRowAdding==false)
                {
                    int rowIndex = dataGridView1.SelectedCells[0].RowIndex;
                    DataGridViewRow editingRow = dataGridView1.Rows[rowIndex];
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[6, rowIndex] = linkCell;
                    editingRow.Cells["Command"].Value = "Update";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "err", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Column_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }


        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column_KeyPress);
            if( dataGridView1.CurrentCell.ColumnIndex == 3)
            {
                TextBox textBox = e.Control as TextBox;
                if (textBox != null)
                {
                    textBox.KeyPress += new KeyPressEventHandler(Column_KeyPress);
                }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Разработано Волжанкиным А.А., группа ПБТ-94", "А кто это сделал?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
