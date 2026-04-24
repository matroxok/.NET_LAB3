# .NET LAB3

Celem laboratorium jest zapoznanie się z podstawami przetwarzania wielowątkowego w technologii
.NET. W ramach zajęć należy stworzyć program w języku C#, który pozwoli na przyspieszenie ob-
liczeń poprzez zrównoleglenie kodu. Będzie ono wykonane z użyciem funkcji zarówno niskiego jak
i wysokiego poziomu, a następnie zostanie zbadane uzyskane przyspieszenie.

## 🛠️ Technologie
- C#
- .NET 8.0
- Entity Framework Core
- SQLite
- System.Text.Json
- Rider (JetBrains)

---

## ⚙️ Środowisko uruchomieniowe
Projekt został wykonany na:
- 💻 **macOS**
- ⚙️ architektura: **ARM (Apple Silicon)**
- 🧠 środowisko IDE: **Rider (JetBrains)**

---

## Pomiary i uruchomienie

Bazowe uruhomienie programu następuje z poziomu IDE lub za pomocą CLI (wtedy, program bazuje na danych przypisanych na stałe w pliku głownym programu)
```c#
const int defaultSize = 100; // romziar macierzy
const int defaultRepetitions = 10;  // liczba powtórzeń pętli
```

## 1) Uruchomienie CLI ( bez argumnetów - wartości bazowe )
```bash
dotnet run --project ConsoleApp.csproj
```

### 1.1) Wyniki dla wartości bazowych

```aiignore
(Parallel vs Wątek)
Wymiar macierzy =>  100 x 100
Ile razy powtórzono? =>  10
Uyte wątki =>  1, 2, 4, 8, 14, 28, 56

Wyniki - Parallel:
   Wątki |               [ms] |  Który szybszy? | Czy zgodne z wątkiem?
---------------------------------------------------------------------------
       1 |               5,57 |            1,00 | tak
       2 |               2,97 |            1,87 | tak
       4 |               1,60 |            3,47 | tak
       8 |               1,01 |            5,53 | tak
      14 |               0,78 |            7,14 | tak
      28 |               0,79 |            7,03 | tak
      56 |               0,66 |            8,40 | tak



--------------------------------------------------------------------------
Wyniki - Thread:
   Wątki |               [ms] |  Który szybszy? | Czy zgodne z wątkiem?
       1 |               5,46 |            1,00 | tak
       2 |               2,88 |            1,90 | tak
       4 |               1,70 |            3,21 | tak
       8 |               1,14 |            4,78 | tak
      14 |               1,13 |            4,83 | tak
      28 |               1,37 |            3,98 | tak
      56 |               2,31 |            2,36 | tak
```

### 1.2) Tablera porównująca wyniki obu metod
```aiignore
Porównanie Parallel vs Thread:
   Wątki |  Parallel [ms] |  Thread [ms] | Szybsze podejście
------------------------------------------------------------------
       1 |           5,57 |         5,46 | Thread
       2 |           2,97 |         2,88 | Thread
       4 |           1,60 |         1,70 | Parallel
       8 |           1,01 |         1,14 | Parallel
      14 |           0,78 |         1,13 | Parallel
      28 |           0,79 |         1,37 | Parallel
      56 |           0,66 |         2,31 | Parallel
```
---
## 2) Uruchomienie CLI ( wraz z arugmentami) 
```bash
dotnet run --project ConsoleApp.csproj -- 400 5 1,2,4,8,16,32
```

### 2.1) Wyniki dla argumentów -- 400 5 1,2,4,8,16,32
```aiignore
(Parallel vs Wątek)
Wymiar macierzy =>  400 x 400
Ile razy powtórzono? =>  5
Uyte wątki =>  1, 2, 4, 8, 16, 32


Wyniki - Parallel:
   Wątki |               [ms] |  Który szybszy? | Czy zgodne z wątkiem?
---------------------------------------------------------------------------
       1 |             354,45 |            1,00 | tak
       2 |             181,07 |            1,96 | tak
       4 |              96,68 |            3,67 | tak
       8 |              49,09 |            7,22 | tak
      16 |              41,29 |            8,58 | tak
      32 |              40,30 |            8,80 | tak

Wyniki - Thread:
   Wątki |               [ms] |  Który szybszy? | Czy zgodne z wątkiem?
---------------------------------------------------------------------------
       1 |             361,75 |            1,00 | tak
       2 |             183,21 |            1,97 | tak
       4 |              96,81 |            3,74 | tak
       8 |              49,32 |            7,34 | tak
      16 |              47,08 |            7,68 | tak
      32 |              40,93 |            8,84 | tak

Porównanie Parallel vs Thread:
   Wątki |  Parallel [ms] |  Thread [ms] | Szybsze podejście
------------------------------------------------------------------
       1 |         354,45 |       361,75 | Parallel
       2 |         181,07 |       183,21 | Parallel
       4 |          96,68 |        96,81 | Parallel
       8 |          49,09 |        49,32 | Parallel
      16 |          41,29 |        47,08 | Parallel
      32 |          40,30 |        40,93 | Parallel
```

### 2.2) Wyniki dla argumentów -- -- 400 5 1,2,4,8,16
```aiignore

(Parallel vs Wątek)
Wymiar macierzy =>  400 x 400
Ile razy powtórzono? =>  5
Uyte wątki =>  1, 2, 4, 8, 16


Wyniki - Parallel:
   Wątki |               [ms] |  Który szybszy? | Czy zgodne z wątkiem?
---------------------------------------------------------------------------
       1 |             354,74 |            1,00 | tak
       2 |             181,37 |            1,96 | tak
       4 |              96,76 |            3,67 | tak
       8 |              49,08 |            7,23 | tak
      16 |              42,49 |            8,35 | tak

Wyniki - Thread:
   Wątki |               [ms] |  Który szybszy? | Czy zgodne z wątkiem?
---------------------------------------------------------------------------
       1 |             359,53 |            1,00 | tak
       2 |             182,50 |            1,97 | tak
       4 |              96,41 |            3,73 | tak
       8 |              49,11 |            7,32 | tak
      16 |              49,87 |            7,21 | tak

Porównanie Parallel vs Thread:
   Wątki |  Parallel [ms] |  Thread [ms] | Szybsze podejście
------------------------------------------------------------------
       1 |         354,74 |       359,53 | Parallel
       2 |         181,37 |       182,50 | Parallel
       4 |          96,76 |        96,41 | Thread
       8 |          49,08 |        49,11 | Parallel
      16 |          42,49 |        49,87 | Parallel
```
---
## 3) Uruchomienie CLI ( wraz z arugmentami i podglądem bierzącym operacji)
```bash
dotnet run --project ConsoleApp.csproj -- 4 2 1,2,4 --show
```

### 3.1) Output z flagą --show
```aiignore
(Parallel vs Wątek)
Wymiar macierzy =>  4 x 4
Ile razy powtórzono? =>  2
Uyte wątki =>  1, 2, 4

Macierz A:
   3   3   9   6
   6   2   9   0
   4   3   2   6
   8   9   6   5

Macierz B:
   7   8   3   0
   0   6   9   5
   6   8   5   0
   7   5   0   8


Wyniki - Parallel:
   Wątki |               [ms] |  Który szybszy? | Czy zgodne z wątkiem?
---------------------------------------------------------------------------
       1 |               0,01 |            1,00 | tak
       2 |               0,23 |            0,03 | tak
       4 |               0,01 |            0,93 | tak

Wyniki - Thread:
   Wątki |               [ms] |  Który szybszy? | Czy zgodne z wątkiem?
---------------------------------------------------------------------------
       1 |               0,05 |            1,00 | tak
       2 |               0,13 |            0,38 | tak
       4 |               0,16 |            0,32 | tak

Porównanie Parallel vs Thread:
   Wątki |  Parallel [ms] |  Thread [ms] | Szybsze podejście
------------------------------------------------------------------
       1 |           0,01 |         0,05 | Parallel
       2 |           0,23 |         0,13 | Thread
       4 |           0,01 |         0,16 | Parallel

Macierz wynikowa:
 117 144  81  63
  96 132  81  10
  82  96  49  63
 127 191 135  85
```
---
## 4) Uruchomienie CLI ( macierz 1000x1000)
```bash
dotnet run --project ConsoleApp.csproj -- 1000 5 1,2,4,8,16 
```

```aiignore
(Parallel vs Wątek)
Wymiar macierzy =>  1000 x 1000
Ile razy powtórzono? =>  5
Uyte wątki =>  1, 2, 4, 8, 16


Wyniki - Parallel:
   Wątki |               [ms] |  Który szybszy? | Czy zgodne z wątkiem?
---------------------------------------------------------------------------
       1 |            5895,22 |            1,00 | tak
       2 |            3060,56 |            1,93 | tak
       4 |            1585,15 |            3,72 | tak
       8 |             806,86 |            7,31 | tak
      16 |             609,53 |            9,67 | tak

Wyniki - Thread:
   Wątki |               [ms] |  Który szybszy? | Czy zgodne z wątkiem?
---------------------------------------------------------------------------
       1 |            6153,63 |            1,00 | tak
       2 |            3120,46 |            1,97 | tak
       4 |            1589,55 |            3,87 | tak
       8 |             807,87 |            7,62 | tak
      16 |             607,83 |           10,12 | tak

Porównanie Parallel vs Thread:
   Wątki |  Parallel [ms] |  Thread [ms] | Szybsze podejście
------------------------------------------------------------------
       1 |        5895,22 |      6153,63 | Parallel
       2 |        3060,56 |      3120,46 | Parallel
       4 |        1585,15 |      1589,55 | Parallel
       8 |         806,86 |       807,87 | Parallel
      16 |         609,53 |       607,83 | Thread
```
---

## Autor
**Mateusz Kozera**  
Nr indeksu: 281801

---

Projekt zrealizowany w ramach kursu:  
**Platformy programistyczne .NET i Java**

🏫 Politechnika Wrocławska  
📚 Kierunek: Informatyczne Systemy Automatyki (ISA)  
🎯 Specjalizacja: IPS