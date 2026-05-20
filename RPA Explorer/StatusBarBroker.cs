using System;
using System.Collections.Generic;
using System.Linq;

namespace RPA_Explorer
{
    public class StatusBarBroker
    {
        private readonly List<StatusBarContext> _statusBarContexts = new List<StatusBarContext>();
        private readonly object _lock = new object();
        private readonly MainWindow _window;

        internal StatusBarBroker(MainWindow window)
        {
            _window = window;
        }

        public StatusBarContext CreateContext()
        {
            var context = new StatusBarContext(this);
            lock (_lock)
            {
                _statusBarContexts.Add(context);
            }
            return context;
        }

        internal void RemoveContext(StatusBarContext context)
        {
            lock (_lock)
            {
                _statusBarContexts.Remove(context);
            }
            UpdateStatus();
        }

        public void UpdateStatus()
        {
            StatusBarContext context = null;
            lock (_lock)
            {
                context = _statusBarContexts.LastOrDefault(ctx => ctx.everTripped);
            }
            string newString; int newValue, newMax;
            if (context != null)
            {
                newString = context.lastString;
                newValue = context.lastValue;
                newMax = context.lastMax;
            }
            else
            {
                newString = Lang.Ready;
                newValue = 0;
                newMax = 100;
            }
            _window.Invoke(() =>
            {
                _window.sblblStatus.Text = newString;
                _window.stsprgProgress.Maximum = newMax;
                _window.stsprgProgress.Value = newValue;
            });
        }
    }

    public class StatusBarContext : IDisposable
    {
        private StatusBarBroker _broker;
        internal bool everTripped;
        internal string lastString;
        internal int lastValue;
        internal int lastMax;

        internal StatusBarContext(StatusBarBroker broker)
        {
            _broker = broker;
        }

        public void Dispose()
        {
            _broker?.RemoveContext(this);
            _broker = null;
        }
        public void UpdateStatus(string newString, int newValue, int newMaximum)
        {
            everTripped = true;
            lastString = newString;
            lastValue = newValue;
            lastMax = newMaximum;
            _broker?.UpdateStatus();
        }
    }
}
