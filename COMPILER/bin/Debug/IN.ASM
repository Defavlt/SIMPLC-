﻿START:
 LDX #$A000
 JMP #PUTA
PUTA:
 LDA #65
 STA ,X
 JMP #End
End:
 END START
 