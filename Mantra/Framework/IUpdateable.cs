using System;

namespace Mantra.Framework
{
    /// <summary>
    /// Defines a method to update logic when enabled.
    /// </summary>
    public interface IUpdateable
    {
        bool Enabled { get; set; }
        void Update();
    }
}
