# Free Blackjack Sim

This is a Blackjack Simulator/Bet spread calculator.
All calculations are done on the fly, which may result in slight deviations from the actual EV, though with 1000+ concurrent players playing 1000+ rounds, the average profits should approach the actual EV. 1000 players and 1000 rounds will run fairly quickly, but have moderate variation in results. 10,000 players and 10,000 rounds leads to consistent results, but takes much longer. Play with these variables until you find an acceptable level of variance in an acceptable amount of time.

# How do I run simulations?
To run for yourself, download BJBlazor.zip, extract, and run BJ_Blazor.exe.
Your console will open, and the first line will look like this:
  info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000

Go to http://localhost:5000 in your browser, and you're done!

# What are the drawbacks?
This is a completely free program managed by just me. I'm not a professional Blackjack player or cardcounter.
Some limitations include:
- No single deck strategy; it will default to double deck (maybe implemented in the future).
- Playing with more than 2 decks results in much lower EV than expected... still working on that issue.
- Strategy assumes H17, selecting an S17 game will play using H17 basic strategy (maybe implementing in the future).
- All calculations are done by performing simulations, there is no pre-calculated EVs, advantage at certain counts, hours until variance is overcame calculations, etc.
