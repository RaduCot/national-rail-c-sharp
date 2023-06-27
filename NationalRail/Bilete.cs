using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace NationalRail
{
    public partial class Bilete : Form
    {
        string tableName = "BILETE"; //AMOGUS
        string identifier = "id_bilet"; //AMOGUS
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

        private void PopulateJourneyComboBox()
        {

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT CALATORII.id_calatorie FROM CALATORII, TRANSPORTURI WHERE CALATORII.id_transport = TRANSPORTURI.id_transport AND TRANSPORTURI.locuri > 0 AND TRANSPORTURI.tip = 'persoane'";

                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string idCalatorie = reader.GetString("id_calatorie");
                        journey_field.Items.Add(idCalatorie);
                    }
                }
            }
        }

        private void PopulateEmployeeComboBox()
        {

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT id_angajat FROM ANGAJATI WHERE functie='casier'";

                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string idAngajat = reader.GetString("id_angajat");
                        employee_field.Items.Add(idAngajat);
                    }
                }
            }
        }

        public Bilete()
        {
            InitializeComponent();
            LoadDataFromDatabase();
            PopulateJourneyComboBox();
            PopulateEmployeeComboBox();
        }

        private void exit_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool CheckColumnHeaders(string[] headers)
        {
            string[] expectedHeaders = { "id_bilet", "data_emitenta", "clasa", "vagon", "loc", "pret", "tip_discount", "id_calatorie", "id_angajat" }; //AMOGUS

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
            string date = date_field.Text;
            string clas = class_field.Text;
            string wagon = wagon_field.Text;
            string seats = seats_field.Text;
            string price = price_field.Text;
            string discount = discount_field.Text;
            string journey = journey_field.Text;
            string employee = employee_field.Text;

            // Validate the input fields
            if (string.IsNullOrEmpty(date) || string.IsNullOrEmpty(clas) || string.IsNullOrEmpty(wagon) || string.IsNullOrEmpty(seats) || string.IsNullOrEmpty(discount) || string.IsNullOrEmpty(journey) || string.IsNullOrEmpty(employee))
            {
                MessageBox.Show("Completați toate câmpurile.", "Atenție", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!double.TryParse(price, out _))
            {
                MessageBox.Show("Salariul trebuie să fie un număr valid.", "Atenție", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Add the new row to the DataGridView
            jTable1.Rows.Add(randomID, date, clas, wagon, seats, price, discount, journey, employee);

            // Clear the input fields
            date_field.Text = "";
            class_field.Text = "";
            wagon_field.Text = "";
            seats_field.Text = "";
            price_field.Text = "";
            discount_field.Text = "";
            journey_field.Text = "";
            employee_field.Text = "";

            status_label.Text = "Status: Date adaugate.";
        }

        private void modify_btn_Click(object sender, EventArgs e)
        {
            if (jTable1.SelectedRows.Count == 1)
            {
                DataGridViewRow selectedRow = jTable1.SelectedRows[0];

                // Get the values from the input fields
                string date = date_field.Text;
                string clas = class_field.Text;
                string wagon = wagon_field.Text;
                string seats = seats_field.Text;
                string price = price_field.Text;
                string discount = discount_field.Text;
                string journey = journey_field.Text;
                string employee = employee_field.Text;

                // Validate the input fields
                if (string.IsNullOrEmpty(date) || string.IsNullOrEmpty(clas) || string.IsNullOrEmpty(wagon) || string.IsNullOrEmpty(seats) || string.IsNullOrEmpty(discount) || string.IsNullOrEmpty(journey) || string.IsNullOrEmpty(employee))
                {
                    MessageBox.Show("Completați toate câmpurile.", "Atenție", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                if (!string.IsNullOrEmpty(price) && !double.TryParse(price, out _))
                {
                    MessageBox.Show("Salariul trebuie să fie un număr valid.", "Atenție", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Update the selected row with the new values
                selectedRow.Cells["data"].Value = date;
                selectedRow.Cells["clasa"].Value = clas;
                selectedRow.Cells["vagon"].Value = wagon;
                selectedRow.Cells["loc"].Value = seats;
                selectedRow.Cells["pret"].Value = price;
                selectedRow.Cells["tip_discount"].Value = discount;
                selectedRow.Cells["id_calatorie"].Value = journey;
                selectedRow.Cells["id_angajat"].Value = employee;

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

        private void price_box_CheckedChanged(object sender, EventArgs e)
        {
            if (price_box.Checked)
            {
                // Apply price filter
                ApplyFilter();
            }
            else
            {
                // Remove price filter and reset DataGridView
                ResetDataGridView();
            }
        }

        private void ApplyFilter()
        {
            // Parse the low and high price values from the textboxes
            if (double.TryParse(low_field.Text, out double lowPrice) && double.TryParse(high_field.Text, out double highPrice))
            {
                // Iterate through each row in the DataGridView and check if the price value is within the specified range
                foreach (DataGridViewRow row in jTable1.Rows)
                {
                    if (row.Cells["pret"].Value != null && double.TryParse(row.Cells["pret"].Value.ToString(), out double price))
                    {
                        // Set the visibility of the row based on the price range
                        row.Visible = (price >= lowPrice && price <= highPrice);
                    }
                    else
                    {
                        // Hide rows with empty or invalid price values
                        row.Visible = false;
                    }
                }
            }
            else
            {
                // Invalid price values entered, reset the DataGridView
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
                date_field.Text = selectedRow.Cells["data_emitenta"].Value.ToString();
                class_field.Text = selectedRow.Cells["clasa"].Value.ToString();
                wagon_field.Text = selectedRow.Cells["vagon"].Value.ToString();
                seats_field.Text = selectedRow.Cells["loc"].Value.ToString();
                price_field.Text = selectedRow.Cells["pret"].Value.ToString();
                discount_field.Text = selectedRow.Cells["tip_discount"].Value.ToString();
                journey_field.Text = selectedRow.Cells["id_calatorie"].Value.ToString();
                employee_field.Text = selectedRow.Cells["id_angajat"].Value.ToString();

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
                            string data = Convert.ToDateTime(row.Cells["data_emitenta"].Value).ToString("yyyy-MM-dd");
                            string clasa = row.Cells["clasa"].Value.ToString();
                            string vagon = row.Cells["vagon"].Value.ToString();
                            string loc = row.Cells["loc"].Value.ToString();
                            string pret = row.Cells["pret"].Value.ToString();
                            string tip_discount = row.Cells["tip_discount"].Value.ToString();
                            string id_calatorie = row.Cells["id_calatorie"].Value.ToString();
                            string id_angajat = row.Cells["id_angajat"].Value.ToString();

                            // Insert the row into the database table
                            string insertQuery = $"INSERT INTO {tableName} ({identifier}, data_emitenta, clasa, vagon, loc, pret, tip_discount, id_calatorie, id_angajat) " +
                                $"VALUES ('{id}', '{data}', '{clasa}', '{vagon}', '{loc}', '{pret}', '{tip_discount}', '{id_calatorie}', '{id_angajat}')";
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

        private void journey_field_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedJourneyId = journey_field.SelectedItem.ToString();

            // Retrieve the wagon count from the database based on the selected journey ID
            int wagonCount = GetWagonCount(selectedJourneyId);

            // Populate the wagon_field ComboBox with values from 1 to wagonCount
            wagon_field.Items.Clear();
            for (int i = 1; i <= wagonCount; i++)
            {
                wagon_field.Items.Add(i);
            }

            // Retrieve the total number of seats from the database based on the selected journey ID
            int totalSeats = GetTotalSeats(selectedJourneyId);

            // Calculate the number of seats per wagon
            int seatsPerWagon = totalSeats / wagonCount;

            // Populate the seats_field ComboBox with values from 1 to seatsPerWagon
            seats_field.Items.Clear();
            for (int i = 1; i <= seatsPerWagon; i++)
            {
                seats_field.Items.Add(i);
            }
        }

        private int GetWagonCount(string journeyId)
        {
            int wagonCount = 0;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT TRENURI.vagoane " +
                               "FROM CALATORII " +
                               "INNER JOIN TRANSPORTURI ON CALATORII.id_transport = TRANSPORTURI.id_transport " +
                               "INNER JOIN TRENURI ON TRANSPORTURI.id_tren = TRENURI.id_tren " +
                               "WHERE CALATORII.id_calatorie = @journeyId";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@journeyId", journeyId);
                connection.Open();

                object result = command.ExecuteScalar();
                if (result != null)
                {
                    wagonCount = Convert.ToInt32(result);
                }
            }

            return wagonCount;
        }

        private int GetTotalSeats(string journeyId)
        {
            int seatCount = 0;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT TRANSPORTURI.locuri " +
                               "FROM CALATORII " +
                               "INNER JOIN TRANSPORTURI ON CALATORII.id_transport = TRANSPORTURI.id_transport " +
                               "WHERE CALATORII.id_calatorie = @journeyId";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@journeyId", journeyId);
                connection.Open();

                object result = command.ExecuteScalar();
                if (result != null)
                {
                    seatCount = Convert.ToInt32(result);
                }
            }

            return seatCount;
        }

        private string GetTransportSpecificatii(string journeyId)
        {
            string specificatii = string.Empty;

            // Assuming you have a MySQL connection named "connection" established
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                // Construct the SQL query
                string query = "SELECT t.specificatii " +
                           "FROM CALATORII c " +
                           "JOIN TRANSPORTURI t ON c.id_transport = t.id_transport " +
                           "WHERE c.id_calatorie = @journeyId";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Add the journeyId parameter to the query
                    command.Parameters.AddWithValue("@journeyId", journeyId);

                    try
                    {
                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Read the specificatii value from the reader
                                specificatii = reader["specificatii"].ToString();
                            }
                        }
                    }
                    catch (MySqlException ex)
                    {
                        // Handle any exceptions that occur during database access
                        Console.WriteLine("Error: " + ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return specificatii;
        }

        private void calculate_btn_Click(object sender, EventArgs e)
        {
            // Get the selected journey ID from journey_field
            string selectedJourneyId = journey_field.Text;

            // Get the specificatii value from TRANSPORTURI table based on the selected journey ID
            string specificatii = GetTransportSpecificatii(selectedJourneyId);

            // Set the base price to 70
            double price = 70;

            // Check if the specificatii is "Personal" and adjust the price accordingly
            if (specificatii == "Personal")
            {
                price /= 2;
            }

            // Get the selected discount from discount_field
            string selectedDiscount = discount_field.SelectedItem.ToString();

            // Adjust the price based on the selected discount
            switch (selectedDiscount)
            {
                case "veteran":
                case "copil":
                    price = 0;
                    break;
                case "student":
                case "pensionar":
                    price /= 2;
                    break;
                case "elev":
                    price /= 4;
                    break;
            }

            // Get the selected class from class_field
            int selectedClass = Convert.ToInt32(class_field.SelectedItem);

            // Adjust the price based on the selected class
            if (selectedClass == 2)
            {
                price /= 2;
            }

            // Display the calculated price
            price_field.Text = price.ToString("0.00");
        }

    }
}
