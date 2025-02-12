# Free Blackjack Sim

This is a Blackjack Simulator/Bet spread calculator.
All calculations are done on the fly, which may result in slight deviations from the actual EV, though with 1000+ concurrent players playing 1000+ rounds, the average profits should approach the actual EV. 1000 players and 1000 rounds will run fairly quickly, but have moderate variation in results. 10,000 players and 10,000 rounds leads to consistent results, but takes much longer. Play with these variables until you find an acceptable level of variance in an acceptable amount of time.

# How do I run simulations?
If you have Visual Studio: copy the file contents or clone the repository to a project and run it.

If you don't have Visual Studio (or don't want to clone the project): download publish.zip, extract the contents, and run BJ_Blazor.exe.
Your console will open, and the first line will look like this:
  info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000

Go to http://localhost:5000 in your browser, and you're done!

# There has to be limitations to a free service... what's the catch?
This is a completely free program managed by just me. I'm not a professional Blackjack player or cardcounter. The only limitations are that some features are a work-in-progress.
Some limitations include:
- Strategy assumes H17. There is no S17, but it is a work-in-progress.
- All calculations are done by performing simulations, there is no pre-calculated EVs, advantage at certain counts, hours until variance is overcame calculations, etc.
- The executable is only available on Windows.
