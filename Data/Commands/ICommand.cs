﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Commands
{
    public interface ICommand
    {
        void Execute(string message);
    }
}
