using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace FFHPWeb
{
    public class test
    {

        //table of products
        private DataTable _productsTable;
        public DataTable ProductsTable
        {
            get
            {
                _productsTable = new DataTable();
                _productsTable.Columns.Add("Product", typeof(string));
                _productsTable.Columns.Add("Id", typeof(int));
                PopulateProductsTable(_productsTable);
                return _productsTable;
            }

        }

        //table of categories
        private DataTable _detailsTable;
        public DataTable DetailsTable
        {
            get
            {
                _detailsTable = new DataTable();
                _detailsTable.Columns.Add("Price", typeof(decimal));
                _detailsTable.Columns.Add("Availability", typeof(bool));
                _detailsTable.Columns.Add("Id", typeof(int));
                PopulateDetailsTable(_detailsTable);
                return _detailsTable;
            }

        }

        public static DataTable PopulateProductsTable(DataTable datatable)
        {
            datatable.Rows.Add("laptop", 1);
            datatable.Rows.Add("desktop", 2);
            datatable.Rows.Add("mobile phone", 3);
            datatable.Rows.Add("mp3 player", 4);
            return datatable;

        }

        public static DataTable PopulateDetailsTable(DataTable datatable)
        {
            datatable.Rows.Add(500, true, 1);
            datatable.Rows.Add(300, true, 2);
            datatable.Rows.Add(100, false, 3);
            datatable.Rows.Add(50, true, 4);
            return datatable;
        }

        
    }
}
