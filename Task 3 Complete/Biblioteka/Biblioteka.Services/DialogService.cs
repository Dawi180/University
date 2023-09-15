﻿using Biblioteka.Controls;
using Biblioteka.Interfaces;

namespace Biblioteka.Services;

public class DialogService : IDialogService
{
    public bool? Show(string itemName)
    {
        ConfirmationDialog confirmationDialog = new ConfirmationDialog(itemName);
        return confirmationDialog.ShowDialog();
    }
}