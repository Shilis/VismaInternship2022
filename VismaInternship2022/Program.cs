using VismaInternship2022;
using VismaInternship2022.Data;

IDataHandler dataHandler = new FileHandler();
AppUI app = new AppUI(dataHandler);
app.Start();