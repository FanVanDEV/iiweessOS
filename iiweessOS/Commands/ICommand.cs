﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iiweessOS.Commands
{
    public interface ICommand
    {
        string Execute(string[] args);
        string GetHelp();
    }
}
