using System;
using System.Collections.Generic;
using System.Text;

namespace KooliProjekt.WpfApplication
{
    public interface IDialogProvider
    {
        bool Confirm(string message);
        void ShowError(string error);
    }
}
