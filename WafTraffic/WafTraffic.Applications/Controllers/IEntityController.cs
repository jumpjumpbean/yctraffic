﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WafTraffic.Applications.Controllers
{
    /// <summary>
    /// Responsible for the database persistence of the entities.
    /// </summary>
    internal interface IEntityController
    {
        bool HasChanges { get; }


        void Initialize();

        bool CanSave();

        bool Save();

        void Shutdown();
    }
}
