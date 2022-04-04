Battleship Game

Poszczególne rodzaje statków dziedziczą z klasy Ship, co jest w tym przypadku naturalne, gdyż statki różnią się jedynie wielkością i nazwą. Upraszcza to tworzenie nowych typów i w przyszłości łatwiejsze rozszerzanie "floty" o nowe statki o innych nazwach i wielkości.
Kod jest zbudowany tak, aby gracze mogli sami określić wymiary planszy.
Na razie wybór oganiczyłem jedynie na froncie od 7x7 do 10x10. Gdybym miał jednak rozwinąć grę w taki sposób, że gracze mogliby również sami określać jakie statki będą wchodzić w skład floty, to stworzyłbym również w backencdzie algorytm, który określi minimalne wymiary planszy dla zadanej floty. Konieczne byłoby to z tego powodu, że zbyt małe wymiary planszy sprawiłyby, że statki zwyczajnie się na niej nie zmieszczą i algorytm, który losowo je rozstawia zapętliłby się.

Numerowanie pól planszy w bazie danych jest w formacie 1,1; 1,2; (...) 2,1; 2,2; (...) itd. zamiast A1; A2; (...) B1; B2; (...).
To rozwiązanie jest moim zdaniem korzystne, gdyż upraszcza korzystanie z bazy i poprawia jej przejrzystość. Oba parametry (nr wiersza i nr kolumny) są tego samego typu (int) więc iterowanie planszy, bądź ogwoływanie się do konkretnego pola jest dużo proszcze. Stosowanie w backendzie liter do numerowania wierszy i liczb do numerowania kolumn (bądź odwrotnie) nie dałoby żadnej korzyści, a jedynie skomplikowało kod. Jeśli już koniecznie chcielibyśmy zmienić ten format, to najlepiej na frontendzie podmienić nr-y wiersza lub kolumny na litery i tak wyświetlić użytkownikowi. W tym celu możnaby np. stworzyć interfejs

export interface Converter
{
    value: int;
    viewValue: string;
}

i tablicę

const conversion: Converter[] = 
[
    { value: 1, viewValue: "A" },
    { value: 2, viewValue: "B" },
    { value: 3, viewValue: "C" },
    { value: 4, viewValue: "D" },
    ...
]

Również dla uproszczenia bazy i wymiany danych nie ma osobnej tabeli [Rows]. Wszystkie pola planszy są przechowywane w tabeli [Fields] z kluczem obcym do tabeli [Board].
Mamy więc jedną tabelę mniej, cop znowu ułatwia odwoływanie się do konkretnego pola i iterowanie wszystkich pól planszy. W celu wyświetlenia użytkownikowi planszy w postaci usystematyzowanych wierszy, model Dto posiada tablicę pól, która stanowi wiersz. Konwersja tablicy pól do tablicy wierszy odbywa się w Api. Dzięki temu na fronie łatwo można poprawnie wyświetlić planszę.

W bazie przechowywane są informacje o ilości punktów gracza, którego gracza jest kolej, w które pola gracze "strzelili" oraz które statki zostały trafione i ile razy. Dzięki temu można w dowolnym monencie kontynuować przerwaną grę.