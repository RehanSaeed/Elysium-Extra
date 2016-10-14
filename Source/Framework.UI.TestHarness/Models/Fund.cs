namespace Framework.UI.TestHarness.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Framework.ComponentModel;
    using Framework.ComponentModel.Rules;
    using Framework.UI.Input;

    /// <summary>
    /// The fund.
    /// </summary>
    public sealed class Fund : NotifyDataErrorInfo<Fund>
    {
        #region Fields

        private readonly DelegateCommand<Fund> moveAboveCommand;
        private readonly DelegateCommand<Fund> moveBelowCommand;
        private readonly DelegateCommand moveDownCommand;
        private readonly DelegateCommand moveUpCommand;

        private readonly FundCollection funds;

        private DateTime date;
        private string icon;
        private bool isActive;
        private string name;
        private string url;
        private int priority;
        private decimal value; 

        #endregion

        #region Constructors

        static Fund()
        {
            Rules.Add(new DelegateRule<Fund>(
                "Date",
                "Date must be more than now",
                x => x.Date >= DateTime.Now));
            Rules.Add(new DelegateRule<Fund>(
                "Icon",
                "Icon cannot be empty",
                x => x.Icon != null));
            Rules.Add(new DelegateRule<Fund>(
                "IsActive",
                "Must be active",
                x => x.IsActive));
            Rules.Add(new DelegateRule<Fund>(
                "Name",
                "Name cannot be empty",
                x => !string.IsNullOrWhiteSpace(x.Name)));
            Rules.Add(new DelegateRule<Fund>(
                "Url",
                "Url cannot be empty",
                x => !string.IsNullOrWhiteSpace(x.Url)));
            Rules.Add(new DelegateRule<Fund>(
                "Value",
                "Value cannot be less than 0",
                x => x.Value >= 0));
        }

        public Fund(FundCollection funds)
        {
            this.funds = funds;

            this.moveAboveCommand = new DelegateCommand<Fund>(this.MoveAbove, this.CanMoveAbove);
            this.moveBelowCommand = new DelegateCommand<Fund>(this.MoveBelow, this.CanMoveBelow);
            this.moveDownCommand = new DelegateCommand(this.MoveDown);
            this.moveUpCommand = new DelegateCommand(this.MoveUp);
        } 

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public DateTime Date
        {
            get { return this.date; }
            set { this.SetProperty(ref this.date, value); }
        }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        public string Icon
        {
            get { return this.icon; }
            set { this.SetProperty(ref this.icon, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        public bool IsActive
        {
            get { return this.isActive; }
            set { this.SetProperty(ref this.isActive, value); }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.SetProperty(ref this.name, value); }
        }

        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        public string Url
        {
            get { return this.url; }
            set { this.SetProperty(ref this.url, value); }
        }

        public int Priority
        {
            get { return this.priority; }
            set { this.SetProperty(ref this.priority, value); }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public decimal Value
        {
            get { return this.value; }
            set { this.SetProperty(ref this.value, value); }
        }

        /// <summary>
        /// Gets the move above command.
        /// </summary>
        /// <value>
        /// The move above command.
        /// </value>
        public DelegateCommand<Fund> MoveAboveCommand
        {
            get { return this.moveAboveCommand; }
        }

        /// <summary>
        /// Gets the move below command.
        /// </summary>
        /// <value>
        /// The move below command.
        /// </value>
        public DelegateCommand<Fund> MoveBelowCommand
        {
            get { return this.moveBelowCommand; }
        }

        /// <summary>
        /// Gets the move down command.
        /// </summary>
        /// <value>
        /// The move down command.
        /// </value>
        public DelegateCommand MoveDownCommand
        {
            get { return this.moveDownCommand; }
        }

        /// <summary>
        /// Gets the move up command.
        /// </summary>
        /// <value>
        /// The move up command.
        /// </value>
        public DelegateCommand MoveUpCommand
        {
            get { return this.moveUpCommand; }
        } 

        #endregion

        #region Private Methods

        /// <summary>
        /// Determines whether this instance can move above the specified fund allocation item.
        /// </summary>
        /// <param name="fund">The fund.</param>
        /// <returns>
        ///   <c>true</c> if this instance can move above the specified fund; otherwise, <c>false</c>.
        /// </returns>
        private bool CanMoveAbove(Fund fund)
        {
            return (this != fund) &&
                fund.CanMove(this.Priority - 1);
        }

        /// <summary>
        /// Moves the specified fund above this instance.
        /// </summary>
        /// <param name="fund">The fund.</param>
        private void MoveAbove(Fund fund)
        {
            if (fund.Priority > this.Priority)
            {
                fund.Move(this.Priority);
            }
            else
            {
                fund.Move(this.Priority - 1);
            }
        }

        /// <summary>
        /// Determines whether this instance can move below the specified fund allocation item.
        /// </summary>
        /// <param name="synonymItem">The synonym item.</param>
        /// <returns>
        /// <c>true</c> if this instance can move below the specified fund allocation item; otherwise, <c>false</c>.
        /// </returns>
        private bool CanMoveBelow(Fund synonymItem)
        {
            return (this != synonymItem) &&
                synonymItem.CanMove(this.Priority + 1);
        }

        /// <summary>
        /// Moves the specified fund allocation item below this instance.
        /// </summary>
        /// <param name="synonymItem">The synonym item.</param>
        private void MoveBelow(Fund synonymItem)
        {
            if (synonymItem.Priority > this.Priority)
            {
                synonymItem.Move(this.Priority + 1);
            }
            else
            {
                synonymItem.Move(this.Priority);
            }
        }

        /// <summary>
        /// Determines whether this instance can move down.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance can move down; otherwise, <c>false</c>.
        /// </returns>
        private bool CanMoveDown()
        {
            return this.CanMove(this.Priority + 1);
        }

        /// <summary>
        /// Moves this instance down.
        /// </summary>
        private void MoveDown()
        {
            if (this.CanMoveDown())
            {
                this.Move(this.Priority + 1);
            }
        }

        /// <summary>
        /// Determines whether this instance can move up.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance can move up; otherwise, <c>false</c>.
        /// </returns>
        private bool CanMoveUp()
        {
            return this.CanMove(this.Priority - 1);
        }

        /// <summary>
        /// Moves this instance up.
        /// </summary>
        private void MoveUp()
        {
            if (this.CanMoveUp())
            {
                this.Move(this.Priority - 1);
            }
        }

        /// <summary>
        /// Determines whether this instance can move this instance to the specified priority.
        /// </summary>
        /// <param name="priority">The priority.</param>
        /// <returns>
        /// <c>true</c> if this instance can move this instance to the specified priority; otherwise, <c>false</c>.
        /// </returns>
        private bool CanMove(int priority)
        {
            List<Fund> fundItems = this.funds.OrderBy(x => x.Priority).ToList();

            int minPriority = fundItems.Min(x => x.Priority);
            if (priority < minPriority)
            {
                priority = minPriority;
            }

            int maxPriority = fundItems.Max(x => x.Priority);
            if (priority > maxPriority)
            {
                priority = maxPriority;
            }

            return (priority >= fundItems.First().Priority) &&
                (priority <= fundItems.Last().Priority) &&
                (((priority <= this.Priority) && (priority != this.Priority)) ||
                ((priority > this.Priority) && (priority != this.Priority)));
        }

        /// <summary>
        /// Moves this instance to the specified priority.
        /// </summary>
        /// <param name="priority">The priority.</param>
        private void Move(int priority)
        {
            List<Fund> fundItems = this.funds.OrderBy(x => x.Priority).ToList();

            int minPriority = fundItems.Min(x => x.Priority);
            if (priority < minPriority)
            {
                priority = minPriority;
            }

            int maxPriority = fundItems.Max(x => x.Priority);
            if (priority > maxPriority)
            {
                priority = maxPriority;
            }

            if (priority <= this.Priority)
            {
                // Move select item up - so shift siblings down
                fundItems
                    .Where(x => (x.Priority >= priority) && (x.Priority < this.Priority))
                    .ToList()
                    .ForEach(x => x.Priority = x.Priority + 1);
            }
            else
            {
                // Move selected item down - so shift siblings up
                fundItems
                    .Where(x => (x.Priority <= priority) && (x.Priority > this.Priority))
                    .ToList()
                    .ForEach(x => x.Priority = x.Priority - 1);
            }

            this.Priority = priority;

            foreach (Fund item in this.funds)
            {
                if (item != this)
                {
                    item.MoveAboveCommand.RaiseCanExecuteChanged();
                    item.MoveBelowCommand.RaiseCanExecuteChanged();
                    item.MoveDownCommand.RaiseCanExecuteChanged();
                    item.MoveUpCommand.RaiseCanExecuteChanged();
                }
            }
        }

        #endregion
    }
}
