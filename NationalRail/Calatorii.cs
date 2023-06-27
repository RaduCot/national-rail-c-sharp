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
    public partial class Calatorii : Form
    {
        string tableName = "CALATORII"; //AMOGUS
        string identifier = "id_calatorie"; //AMOGUS
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
        private void PopulateCities()
        {
            // Read the CSV file
            string csvFilePath = Path.Combine(Application.StartupPath, "csvfiles", "orase.csv");
            string[] lines = File.ReadAllLines(csvFilePath);

            // Parse the CSV data and create separate lists for departure and arrival cities
            List<string> departureCities = new List<string>();
            List<string> arrivalCities = new List<string>();

            foreach (string line in lines)
            {
                string[] columns = line.Split(',');

                if (columns.Length >= 3)
                {
                    string city = $"{columns[2]}, {columns[1]}";
                    departureCities.Add(city);
                    arrivalCities.Add(city);
                }
            }

            // Bind the data to the ComboBoxes and enable auto-complete
            departure_field.DataSource = departureCities;
            arrival_field.DataSource = arrivalCities;
        }


        private void PopulateTransportComboBox()
        {

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT id_transport FROM TRANSPORTURI";

                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string idCalatorie = reader.GetString("id_transport");
                        transport_field.Items.Add(idCalatorie);
                    }
                }
            }
        }

        public Calatorii()
        {
            InitializeComponent();
            departure_time.Format = DateTimePickerFormat.Custom;
            departure_time.CustomFormat = "HH:mm:ss";
            arrival_time.Format = DateTimePickerFormat.Custom;
            arrival_time.CustomFormat = "HH:mm:ss";
            LoadDataFromDatabase();
            PopulateCities();
            PopulateTransportComboBox();
        }

        private void exit_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool CheckColumnHeaders(string[] headers)
        {
            string[] expectedHeaders = { "id_calatorie", "data_plecare", "ora_plecare", "data_sosire", "ora_sosire", "loc_plecare", "loc_sosire", "id_transport"}; //AMOGUS

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

        private void add_btn_Click(object sender, EventArgs e)
        {
            // Generate a random ID between 999 and 10000
            int randomID;
            do
            {
                randomID = new Random().Next(999, 10000);
            } while (IsIDExists(randomID));

            // Get the values from the input fields
            string data_plecare = departure_date.Text;
            string ora_plecare = departure_time.Text;
            string data_sosire = arrival_date.Text;
            string ora_sosire = arrival_time.Text;
            string loc_plecare = departure_field.Text;
            string loc_sosire = arrival_field.Text;
            string id_transport = transport_field.Text;

            // Validate the input fields
            if (string.IsNullOrEmpty(data_plecare) || string.IsNullOrEmpty(ora_plecare) || string.IsNullOrEmpty(data_sosire) || string.IsNullOrEmpty(ora_sosire) || string.IsNullOrEmpty(loc_plecare) || string.IsNullOrEmpty(loc_sosire) || string.IsNullOrEmpty(id_transport))
            {
                MessageBox.Show("Completați toate câmpurile.", "Atenție", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            // Add the new row to the DataGridView
            jTable1.Rows.Add(randomID, data_plecare, ora_plecare, data_sosire, ora_sosire, loc_plecare, loc_sosire, id_transport);

            // Clear the input fields
            id_field.Text = "";
            departure_date.Text = "";
            departure_time.Text = "";
            arrival_date.Text = "";
            arrival_time.Text = "";
            departure_field.Text = "";
            arrival_field.Text = "";
            transport_field.Text = "";

            status_label.Text = "Status: Date adaugate.";
        }

        private void modify_btn_Click(object sender, EventArgs e)
        {
            if (jTable1.SelectedRows.Count == 1)
            {
                DataGridViewRow selectedRow = jTable1.SelectedRows[0];

                // Get the values from the input fields
                string data_plecare = departure_date.Text;
                string ora_plecare = departure_time.Text;
                string data_sosire = arrival_date.Text;
                string ora_sosire = arrival_time.Text;
                string loc_plecare = departure_field.Text;
                string loc_sosire = arrival_field.Text;
                string id_transport = transport_field.Text;

                // Validate the input fields
                if (string.IsNullOrEmpty(data_plecare) || string.IsNullOrEmpty(ora_plecare) || string.IsNullOrEmpty(data_sosire) || string.IsNullOrEmpty(ora_sosire) || string.IsNullOrEmpty(loc_plecare) || string.IsNullOrEmpty(loc_sosire) || string.IsNullOrEmpty(id_transport))
                {
                    MessageBox.Show("Completați toate câmpurile.", "Atenție", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Update the selected row with the new values
                selectedRow.Cells["data_plecare"].Value = data_plecare;
                selectedRow.Cells["ora_plecare"].Value = ora_plecare;
                selectedRow.Cells["data_sosire"].Value = data_sosire;
                selectedRow.Cells["ora_sosire"].Value = ora_sosire;
                selectedRow.Cells["loc_plecare"].Value = loc_plecare;
                selectedRow.Cells["loc_sosire"].Value = loc_sosire;
                selectedRow.Cells["id_transport"].Value = id_transport;


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

        private void jTable1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                jTable1.Rows[e.RowIndex].Selected = true;

                DataGridViewRow selectedRow = jTable1.Rows[e.RowIndex];

                id_field.Text = selectedRow.Cells[$"{identifier}"].Value.ToString();
                departure_date.Text = selectedRow.Cells["data_plecare"].Value.ToString();
                departure_time.Text = selectedRow.Cells["ora_plecare"].Value.ToString();
                arrival_date.Text = selectedRow.Cells["data_sosire"].Value.ToString();
                arrival_time.Text = selectedRow.Cells["ora_sosire"].Value.ToString();
                departure_field.Text = selectedRow.Cells["loc_plecare"].Value.ToString();
                arrival_field.Text = selectedRow.Cells["loc_sosire"].Value.ToString();
                transport_field.Text = selectedRow.Cells["id_transport"].Value.ToString();
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
                            string data_plecare = Convert.ToDateTime(row.Cells["data_plecare"].Value).ToString("yyyy-MM-dd");
                            string ora_plecare = TimeSpan.Parse(row.Cells["ora_sosire"].Value.ToString()).ToString("hh\\:mm\\:ss");
                            string data_sosire = Convert.ToDateTime(row.Cells["data_sosire"].Value).ToString("yyyy-MM-dd");
                            string ora_sosire = TimeSpan.Parse(row.Cells["ora_sosire"].Value.ToString()).ToString("hh\\:mm\\:ss");
                            string loc_plecare = row.Cells["loc_plecare"].Value.ToString();
                            string loc_sosire = row.Cells["loc_sosire"].Value.ToString();
                            string id_transport = row.Cells["id_transport"].Value.ToString();

                            // Insert the row into the database table
                            string insertQuery = $"INSERT INTO {tableName} ({identifier}, data_plecare, ora_plecare, data_sosire, ora_sosire, loc_plecare, loc_sosire, id_transport) " +
                                $"VALUES ('{id}', '{data_plecare}', '{ora_plecare}', '{data_sosire}', '{ora_sosire}', '{loc_plecare}', '{loc_sosire}', '{id_transport}')";
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
