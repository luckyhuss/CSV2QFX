﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSV2QFX.Stub
{
    class AccountItem
    {
        public string BankId { get; }
        public string AcctNo { get; }
        public string AcctType { get; }
        public string AcctCurr { get; }

        public AccountItem(string bankId, string accNo, string acctType, string acctCurr)
        {
            BankId = bankId;
            AcctNo = accNo;
            AcctType = acctType;
            AcctCurr = acctCurr;
        }

        public override string ToString()
        {
            return String.Format("{0} - {2} - {1}", AcctNo, AcctCurr, AcctType);
        }
    }
}
