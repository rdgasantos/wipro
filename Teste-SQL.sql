/*PERGUNTA 1*/
SELECT s.dsStatus,COUNT(*) 
FROM tb_Status s
INNER JOIN tb_Processo p ON s.idStatus = p.idStatus
GROUP BY s.dsStatus

/*PERGUNTA 2*/

SELECT p.nroProcesso, MAX(a.dtAndamento) 
FROM tb_Andamento a
INNER JOIN tb_Processo p ON  a.idProcesso = p.idProcesso
WHERE YEAR(p.DtEncerramento) = '2013'

/*PERGUNTA 3*/

SELECT p.DtEncerramento, COUNT(p.DtEncerramento)
FROM tb_Processo p
GROUP BY p.DtEncerramento
HAVING COUNT(p.DtEncerramento)> 5

/*PERGUNTA 4*/

SELECT REPLICATE('0',12 - LEN(nroProcesso)) + RTRIM(nroProcesso) FROM tb_Processo
