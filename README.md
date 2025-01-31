# Free Blackjack Sim

This is a Blackjack Simulator/Bet spread calculator.
All calculations are done on the fly, which may result in slight deviations from the actual EV, though with 1000+ concurrent players playing 1000+ rounds, the average profits should approach the actual EV.

All changes to the strategies, rules, and spreads must be made inside the software at this time.
The following features (and more) are not fully implemented:
- DAS (is always on)
- Split Count limit on any card (currently, the number of splits is determined by how many splits you can get in a row)
- Dealer Peeks for blackjack (always on)
- Deviations (some code exists, but is not yet implemented or completed)
- Rules determined from outside the code (all rules, spreads, bankrolls, etc. must be changed inside the code, though it should be simple enough to do so)
