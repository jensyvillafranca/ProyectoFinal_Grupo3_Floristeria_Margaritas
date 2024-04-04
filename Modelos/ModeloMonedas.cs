using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
   public  class ModeloMonedas : Entry
    {
        public static readonly BindableProperty CurrencyCodeProperty =
            BindableProperty.Create(nameof(CurrencyCode), typeof(string), typeof(ModeloMonedas), "USD");

        public string CurrencyCode
        {
            get => (string)GetValue(CurrencyCodeProperty);
            set => SetValue(CurrencyCodeProperty, value);
        }

        public ModeloMonedas()
        {
            TextChanged += OnTextChanged;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                if (decimal.TryParse(e.NewTextValue, out decimal value))
                {
                    Text = value.ToString("C", CultureInfo.CreateSpecificCulture(CurrencyCode));
                }
                else
                {
                    Text = e.OldTextValue;
                }
            }
        }
    }
}
