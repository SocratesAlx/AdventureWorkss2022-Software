using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



// den to xrhsimopoiw , einai edw apla gia testing gia animations


namespace SokProodos
{
    public static class TransitionManager
    {
        public static async Task ShowWithFade(Form newForm, Form currentForm = null)
        {
            newForm.Opacity = 0;
            newForm.Show();

            while (newForm.Opacity < 1)
            {
                newForm.Opacity += 0.05;
                await Task.Delay(10);
            }

            if (currentForm != null)
            {
                while (currentForm.Opacity > 0)
                {
                    currentForm.Opacity -= 0.05;
                    await Task.Delay(10);
                }
                currentForm.Close();
            }
        }
    }
}
