﻿using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using testing.Models;
using testing.DataModels;

namespace testing.ViewModels
{
    public class VM_DefaultPage : testing.BaseViewModel
    {
        public String ConsoleMessage { get; set; } = "Konichiwa Desu";
        public String ExampleTitle { get; set; } = "Konichiwa Desu";
        public String Yeeted { get; set; } = "Konichiwa Desu";
        public ObservableCollection<PageTabModel> TabBinding { get; set; }
        public ICommand SaveVideo { get; set; }

        public VM_DefaultPage() {
            SaveVideo = new RelayCommand(o => { AllIs(); },
                                        o => true );

            TabBinding = new ObservableCollection<PageTabModel>();

            TabBinding.Add(new PageTabModel("Welcome", EPageList.CombineTexture, true));
            TabBinding[0].IsSelected = true;
        }

        private void AllIs() {
            TabBinding.Add(new PageTabModel("Content", EPageList.PlayVideo));
        }
    }
}