using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ftCarWin
{
    /// <summary>
    /// An interface for docked forms
    /// </summary>
    public interface IDockedForm
    {
        /// <summary>
        ///  Reinitialize the handlers of this form
        /// </summary>
        void InitHandlers();

        /// <summary>
        /// Show the form
        /// </summary>
        void Show();
    }
}
