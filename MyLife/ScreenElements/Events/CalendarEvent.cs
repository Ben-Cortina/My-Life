#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace MyLife
{
    public class CalendarEvent
    {
        #region Fields and Properties

        bool completed = false;

        public bool Completed
        {
            get { return completed; }
            set { completed = value; }
        }

        DateTime dueDate;

        /// <summary>
        /// Date the Event is Due
        /// </summary>
        public DateTime DueDate
        {
            get { return dueDate; }
            set { dueDate = value; }
        }

        /// <summary>
        /// Name to be displayed on reminder
        /// </summary>
        String name;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        int amount;

        /// <summary>
        /// Amount Due
        /// </summary>
        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        #endregion

        #region Events

        public event EventHandler<PlayerIndexEventArgs> Due;

        /// <summary>
        /// Method for raising the Selected event.
        /// </summary>
        protected internal virtual void OnDue(PlayerIndex playerIndex)
        {
            if (Due != null)
                Due(this, new PlayerIndexEventArgs(playerIndex));
        }


        public void DueCheck()
        {
            PlayerIndex e = new PlayerIndex();
            if (dueDate == GlobalVariables.Date)
                OnDue(e);
        }
        #endregion
    }
}
