# NemoProjects_MainManager
NemoProjects_MainManager è un launcher e gestore centrale per tutti i progetti della suite NemoProjects.
Permette di:

- Eseguire applicazioni (in .exe, .dll, .jar, .py) da una lista preconfigurata o da una cartella.
- Lanciare un generatore di template Python per creare nuovi progetti NemoProjects_<Lang>.
- Gestire in modo semplice e interattivo l’avvio e l’organizzazione dei propri progetti da un’unica interfaccia a riga di comando.
- 
Il progetto combina C# per la parte di gestione/avvio e Python per la generazione automatica di strutture di progetto.
## struttura base
```
  NemoProjects.MainManager/
  │
  ├── Program.cs                # punto di ingresso: avvia il menu principale
  ├── Menu.cs                   # gestisce il menu interattivo e la logica principale
  ├── Runner.cs                 # si occupa di lanciare i processi esterni
  ├── AppInfo.cs                # modello dati per rappresentare un’applicazione
  │
  ├── apps.txt                  # file di configurazione con l’elenco delle app eseguibili
  ├── banner.txt                # banner ASCII visualizzato all’avvio
  │
  ├── NemoProjects_template.py  # script Python per generare template di progetto
  └── README.md                 # questo file :)
```
## menu
ha un menu principale con le seguenti voci

1. usa lista eseguibili da apps.txt
2. scansiona una cartella
3. esegui script di creazione NemoProjects_"Lang"
0. Esci

## Scansione directory
Permette di cercare eseguibili all’interno di una cartella, riconoscendo estensioni .exe, .jar, .py, .dll.
Ogni file trovato viene aggiunto alla lista delle app lanciabili.

Il programma riconosce automaticamente il tipo di file e genera il comando corretto:

-.exe → avvia direttamente
- .jar → java -jar ...
- .py → python ...
- .dll → prova con dotnet ...

## Esecuzione da apps.txt

Permette di avviare rapidamente applicazioni predefinite tramite un file di configurazione,
guarda apps.txt per vedere come aggiungerli, in linea di massima la struttura è 
```
<NOME_APP> | <COMANDO E/O PERCORSO_ESEGUIBILE>
```

## script python Generatore di progetti NemoProjects_<Lang>
Quando si seleziona l’opzione 3, viene lanciato lo script Python NemoProjects_template.py, che genera automaticamente strutture di progetti per diversi linguaggi:

- C
- Java
- C#
- Python

Per ogni linguaggio, crea:

- una cartella base NemoProjects_<Lang>
- un readme generico con la lista dei progetti individuali
- progetti individuali (Calculator, Converter, Hangman, ecc.)
- sottocartelle specifiche nei progetti individuali (src, include, ecc.)
- un README.md per ogni progetto con istruzioni per la creazione e l’avvio


