using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace NationalRail
{
    public partial class Angajati : Form
    {
        string tableName = "ANGAJATI"; //AMOGUS
        string identifier = "id_angajat"; //AMOGUS
        string connectionString = "Server=aws.connect.psdb.cloud;Database=nationalrail;user=8nkamwr4mg3r220qby71;password=pscale_pw_GKLKnsX2kmZksbDnV5WKXKAk1myumGcreFNjQiptAJK;SslMode=VerifyFull;";
        private void LoadDataFromDatabase()
        {
            string query = $"SELECT * FROM {tableName};";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();

                    // Clear any existing data in the DataGridView
                    jTable1.Rows.Clear();

                    // Loop through the retrieved data and add new rows to the DataGridView
                    while (reader.Read())
                    {
                        // Create an array to store the values for each column
                        object[] rowData = new object[reader.FieldCount];

                        // Retrieve the values from the database columns
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            rowData[i] = reader[i];
                        }

                        // Add a new row with the retrieved values
                        jTable1.Rows.Add(rowData);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that occur during the database connection or data retrieval process
                    MessageBox.Show("A apărut o eroare: " + ex.Message, "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public Angajati()
        {
            InitializeComponent();
            LoadDataFromDatabase();
        }

        private void exit_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool CheckColumnHeaders(string[] headers)
        {
            string[] expectedHeaders = { "id_angajat", "nume", "prenume", "cnp", "functie", "salariu" }; //AMOGUS

            // Check if the number of headers matches
            if (headers.Length != expectedHeaders.Length)
            {
                return false;
            }

            // Check if the headers match the expected names
            for (int i = 0; i < headers.Length; i++)
            {
                if (headers[i] != expectedHeaders[i])
                {
                    return false;
                }
            }

            return true;
        }

        private void import_btn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                string[] lines = File.ReadAllLines(filePath);

                // Assuming the first line contains the column headers,
                // split it to get the column names
                string[] headers = lines[0].Split(',');

                // Check if the column headers match the expected names
                bool columnHeadersMatch = CheckColumnHeaders(headers);

                if (columnHeadersMatch)
                {
                    // Clear any existing data in the DataGridView
                    jTable1.Rows.Clear();
                    jTable1.Columns.Clear();

                    // Add columns to the DataGridView based on the headers
                    foreach (string header in headers)
                    {
                        jTable1.Columns.Add(header, header);
                    }

                    // Skip the first line (column headers) and populate the DataGridView
                    for (int i = 1; i < lines.Length; i++)
                    {
                        string[] fields = lines[i].Split(',');
                        jTable1.Rows.Add(fields);
                    }
                    status_label.Text = "Status: Date importate.";
                }
                else
                {
                    MessageBox.Show("Fișierul CSV importat nu are coloanele așteptate.", "Eroare Import", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void export_btn_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV Files (*.csv)|*.csv";
            saveFileDialog.FileName = $"{tableName}.csv";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                StringBuilder sb = new StringBuilder();

                // Append column headers to the CSV file
                for (int i = 0; i < jTable1.Columns.Count; i++)
                {
                    sb.Append(jTable1.Columns[i].HeaderText);

                    if (i < jTable1.Columns.Count - 1)
                    {
                        sb.Append(",");
                    }
                }

                sb.AppendLine();

                // Append rows to the CSV file
                foreach (DataGridViewRow row in jTable1.Rows)
                {
                    for (int i = 0; i < jTable1.Columns.Count; i++)
                    {
                        sb.Append(row.Cells[i].Value);

                        if (i < jTable1.Columns.Count - 1)
                        {
                            sb.Append(",");
                        }
                    }

                    sb.AppendLine();
                }

                // Write the CSV data to the file
                File.WriteAllText(filePath, sb.ToString());

                MessageBox.Show("Datele s-au exportat cu succes.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                status_label.Text = "Status: Date exportate.";
            }
        }

        private void deselect_btn_Click(object sender, EventArgs e)
        {
            if (jTable1.SelectedRows.Count > 0)
            {
                jTable1.ClearSelection();
                status_label.Text = "Status: Rânduri deselectate.";
            }
        }

        private void delete_btn_Click(object sender, EventArgs e)
        {
            if (jTable1.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Sigur doriți să ștergeți rândurile selectate?", "Confirmare", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    status_label.Text = "Status: Date șterse.";
                    foreach (DataGridViewRow row in jTable1.SelectedRows)
                    {
                        jTable1.Rows.Remove(row);
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecția este nulă.", "Informație", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool IsIDExists(int id)
        {
            foreach (DataGridViewRow row in jTable1.Rows)
            {
                int existingID;
                if (int.TryParse(row.Cells[$"{identifier}"].Value.ToString(), out existingID))
                {
                    if (existingID == id)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsCNPExists(string cnp)
        {
            foreach (DataGridViewRow row in jTable1.Rows)
            {
                if (row.Cells["cnp"].Value != null && row.Cells["cnp"].Value.ToString() == cnp) //AMOGUS, existing value field
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsNumericCNP(string cnp)
        {
            if (cnp.Length != 10)
                return false;

            foreach (char c in cnp)
            {
                if (!char.IsDigit(c))
                    return false;
            }

            return true;
        }

        private void add_btn_Click(object sender, EventArgs e)
        {
            // Generate a random ID between 999 and 10000
            int randomID;
            do
            {
                randomID = new Random().Next(999, 10000);
            } while (IsIDExists(randomID));

            // Get the values from the input fields
            string name = name_field.Text; //AMOGUS
            string surname = surname_field.Text;
            string cnp = cnp_field.Text;
            string function = function_field.Text;
            string salary = salary_field.Text;

            // Validate the input fields
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(function) || string.IsNullOrEmpty(salary))
            {
                MessageBox.Show("Completați toate câmpurile.", "Atenție", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsNumericCNP(cnp))
            {
                MessageBox.Show("CNP-ul trebuie să fie un identificator de 10 cifre.", "Atenție", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!double.TryParse(salary, out _))
            {
                MessageBox.Show("Salariul trebuie să fie un număr valid.", "Atenție", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if CNP already exists in the DataGridView
            if (IsCNPExists(cnp))
            {
                MessageBox.Show("CNP-ul deja există. Introduceți un CNP valid.", "Atenție", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Add the new row to the DataGridView
            jTable1.Rows.Add(randomID, name, surname, cnp, function, salary);

            // Clear the input fields
            id_field.Text = "";
            name_field.Text = "";
            surname_field.Text = "";
            cnp_field.Text = "";
            function_field.Text = "";
            salary_field.Text = "";

            status_label.Text = "Status: Date adaugate.";
        }

        private void modify_btn_Click(object sender, EventArgs e)
        {
            if (jTable1.SelectedRows.Count == 1)
            {
                DataGridViewRow selectedRow = jTable1.SelectedRows[0];

                // Get the values from the input fields
                string name = name_field.Text;
                string surname = surname_field.Text;
                string cnp = cnp_field.Text;
                string function = function_field.Text;
                string salary = salary_field.Text;

                // Validate the input fields
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(function) || string.IsNullOrEmpty(salary))
                {
                    MessageBox.Show("Completați toate câmpurile.", "Atenție", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!string.IsNullOrEmpty(cnp) && !IsNumericCNP(cnp))
                {
                    MessageBox.Show("CNP-ul trebuie să fie un identificator de 10 cifre.", "Atenție", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!string.IsNullOrEmpty(salary) && !double.TryParse(salary, out _))
                {
                    MessageBox.Show("Salariul trebuie să fie un număr valid.", "Atenție", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Update the selected row with the new values
                selectedRow.Cells["nume"].Value = name;
                selectedRow.Cells["prenume"].Value = surname;
                selectedRow.Cells["cnp"].Value = cnp;
                selectedRow.Cells["functie"].Value = function;
                selectedRow.Cells["salariu"].Value = salary;

                status_label.Text = "Status: Date modificate.";
            }
            else
            {
                MessageBox.Show("Selectați un singur rând pentru a aplica modificări.", "Atenție", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void search_field_TextChanged(object sender, EventArgs e)
        {
            string searchText = search_field.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                // If the search field is empty, reset the DataGridView to its original state
                ResetDataGridView();
            }
            else
            {
                // Perform the search and filter the DataGridView based on the search text
                SearchAndFilterDataGridView(searchText);
            }
        }

        private void ResetDataGridView()
        {
            // Clear any existing filters and show all rows in the DataGridView
            jTable1.ClearSelection();
            foreach (DataGridViewRow row in jTable1.Rows)
            {
                row.Visible = true;
            }
        }

        private void SearchAndFilterDataGridView(string searchText)
        {
            // Iterate through each row in the DataGridView and check if it contains the search text
            foreach (DataGridViewRow row in jTable1.Rows)
            {
                bool rowContainsSearchText = false;

                // Iterate through each cell in the current row and check if it contains the search text
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && cell.Value.ToString().IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        rowContainsSearchText = true;
                        break;
                    }
                }

                // Set the visibility of the row based on whether it contains the search text or not
                row.Visible = rowContainsSearchText;
            }
        }

        private void salary_box_CheckedChanged(object sender, EventArgs e)
        {
            if (salary_box.Checked)
            {
                // Apply salary filter
                ApplyFilter();
            }
            else
            {
                // Remove salary filter and reset DataGridView
                ResetDataGridView();
            }
        }

        private void ApplyFilter()
        {
            // Parse the low and high salary values from the textboxes
            if (double.TryParse(low_field.Text, out double lowSalary) && double.TryParse(high_field.Text, out double highSalary))
            {
                // Iterate through each row in the DataGridView and check if the salary value is within the specified range
                foreach (DataGridViewRow row in jTable1.Rows)
                {
                    if (row.Cells["salariu"].Value != null && double.TryParse(row.Cells["salariu"].Value.ToString(), out double salary))
                    {
                        // Set the visibility of the row based on the salary range
                        row.Visible = (salary >= lowSalary && salary <= highSalary);
                    }
                    else
                    {
                        // Hide rows with empty or invalid salary values
                        row.Visible = false;
                    }
                }
            }
            else
            {
                // Invalid salary values entered, reset the DataGridView
                ResetDataGridView();
            }
        }

        private void jTable1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                jTable1.Rows[e.RowIndex].Selected = true;

                DataGridViewRow selectedRow = jTable1.Rows[e.RowIndex];

                id_field.Text = selectedRow.Cells[$"{identifier}"].Value.ToString();
                name_field.Text = selectedRow.Cells["nume"].Value.ToString();
                surname_field.Text = selectedRow.Cells["prenume"].Value.ToString();
                cnp_field.Text = selectedRow.Cells["cnp"].Value.ToString();
                function_field.Text = selectedRow.Cells["functie"].Value.ToString();
                salary_field.Text = selectedRow.Cells["salariu"].Value.ToString();
            }
        }

        private void jButton1_Click(object sender, EventArgs e)
        {
            LoadDataFromDatabase();
            status_label.Text = "Status: Date reîncărcate.";
        }

        private void save_btn_Click(object sender, EventArgs e)
        {

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Delete all existing rows from the database table
                    string deleteQuery = $"DELETE FROM {tableName}";
                    ExecuteNonQuery(connection, deleteQuery);

                    // Iterate over the rows in the DataGridView
                    foreach (DataGridViewRow row in jTable1.Rows)
                    {
                        // Skip the last row if it's the new row
                        if (!row.IsNewRow)
                        {
                            // Get the values from the DataGridView cells
                            string id = row.Cells[$"{identifier}"].Value.ToString();
                            string nume = row.Cells["nume"].Value.ToString();
                            string prenume = row.Cells["prenume"].Value.ToString();
                            string cnp = row.Cells["cnp"].Value.ToString();
                            string functie = row.Cells["functie"].Value.ToString();
                            string salariu = row.Cells["salariu"].Value.ToString();

                            // Insert the row into the database table
                            string insertQuery = $"INSERT INTO {tableName} ({identifier}, nume, prenume, cnp, functie, salariu) " +
                                $"VALUES ('{id}', '{nume}', '{prenume}', '{cnp}', '{functie}', '{salariu}')";
                            ExecuteNonQuery(connection, insertQuery);
                        }
                    }

                    MessageBox.Show("Datele au fost salvate cu succes.", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    status_label.Text = "Status: Date salvate in MySQL.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la salvarea datelor: " + ex.Message, "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExecuteNonQuery(MySqlConnection connection, string query)
        {
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }

    }
}
