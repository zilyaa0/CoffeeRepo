﻿using DesctopForCafe.Services.DbService;
using DesctopForCafe.Views;
using DXFToPGApp.MVVMTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DesctopForCafe.ViewModel
{
    public class ProductsVM : ViewModelBase
    {
        public ProductsVM(int groupId)
        {
            var dbService = new DbService();
            Products = dbService.GetProducts(groupId);
            Group = dbService.GetGroupNameById(groupId);
        }
        private List<ProductsData> _products;
        public List<ProductsData> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                OnPropertyChanged("Products");
            }
        }
        private string _groupName;
        public string Group
        {
            get { return _groupName; }
            set
            {
                _groupName = value;
                OnPropertyChanged("Group");
            }
        }
        private ProductsData _selectedProduct;
        public ProductsData SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                OnPropertyChanged("SelectedProduct");
            }
        }
        public ICommand OpenItem

        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    Continue();
                });
            }
        }

        private bool CanContinue()
        {
            return SelectedProduct != null;
        }

        private void Continue()
        {
            if (!CanContinue())
                return;
            var products = new SelectedProductView();
            var vm = new SelectedProductVM(SelectedProduct);
            products.DataContext = vm;
            products.ShowDialog();

        }
    }
}
