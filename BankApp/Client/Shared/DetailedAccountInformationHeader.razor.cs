using BankApp.Shared.Entities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankApp.Client.Shared
{
    public partial class DetailedAccountInformationHeader
    {
        [Parameter] public BankAccount BankAccount { get; set; }
    }
}
