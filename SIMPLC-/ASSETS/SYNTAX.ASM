START
 LDA 63 12
 STA X

; Video memory starts at
; address 4096, and ends at
; address 8192

; We have two registers available:
; A, B
; But also one special, memory-pointer:
; X

; Example:

START
 LDA 65
 LDX 4096
 STA X
 END START

; In other words:
; function is START
; LOAD register A with 65
; LOAD register X with 4096
; STORE register A at address X
; END program
; EXECUTE PROGRAM