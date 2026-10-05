# 26.02 Windows Forms harjutused

Kolm eraldi programmi, kohandatud lahenduse .NET 8 versioonile. Vormide paigutus luuakse Form1 konstruktoris. Iga projekti saab käivitada eraldi, WebAPI pole vajalik.

- PictureViewer: pildi avamine, tühjendamine, venitamine, taustavärvi valimine ja sulgemine.
- MathQuiz: liitmine, lahutamine, korrutamine ja täisarvuline jagamine; 30 sekundi taimer; võidu ja aja lõppemise teated; korduv mäng.
- MatchingGame: kaheksa juhuslikult paigutatud paari; sobimatu paari peitmine 750 ms pärast; sobivate paaride säilitamine; võiduteade ja uus mäng.

## Kontrollimine

1. Ava pilt, lülita venitamine sisse ja välja, muuda taustavärvi ja tühjenda pilt. Proovi avada ka sobimatu fail.
2. Alusta viktoriini, lahenda kõik neli tehet, seejärel alusta uuesti ja lase ajal lõppeda.
3. Ava mängus kaks erinevat ikooni, kontrolli nende peitmist; leia sobiv paar ja lõpuks kõik paarid. Proovi ka uut mängu.

## Microsofti õppematerjalid

- https://learn.microsoft.com/en-us/visualstudio/get-started/csharp/tutorial-windows-forms-picture-viewer-layout
- https://learn.microsoft.com/en-us/visualstudio/get-started/csharp/tutorial-windows-forms-math-quiz-create-project-add-controls
- https://learn.microsoft.com/en-us/visualstudio/get-started/csharp/tutorial-windows-forms-create-match-game
