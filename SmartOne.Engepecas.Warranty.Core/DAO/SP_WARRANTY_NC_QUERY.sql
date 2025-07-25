ALTER PROCEDURE SP_WARRANTY_NC_QUERY
(
	filterType	nvarchar(2),
	number		nvarchar(50)
)
LANGUAGE SQLSCRIPT
AS
BEGIN

	SELECT
		OSCL."callID" "Id",
		SCL6."SrcvCallID" || '/' || SCL6."Line" "Atendimento",
		CASE 
			WHEN SCL6."U_ENG_Tipo" = 'Atendimento KM' THEN TO_DECIMAL(SCL6."U_ENG_KM", 10, 2) || ' KM'
			WHEN SCL6."U_ENG_Tipo" = 'Atendimento HR' THEN TO_DECIMAL(SCL6."ActualDur", 10, 2) || ' HT'
			ELSE SCL6."U_ENG_Tipo" || ' - ' || IFNULL(SCL6."Remark", '')
		END "Descrição",
		MULT."U_Mult"	"Multiplicador",
		CASE 
			WHEN SCL6."U_ENG_Tipo" = 'Atendimento KM' THEN SCL6."U_ENG_KM" * MULT."U_Mult"
			WHEN SCL6."U_ENG_Tipo" = 'Atendimento HR' THEN SCL6."ActualDur" * MULT."U_Mult"
			ELSE SCL6."U_ENG_NFValor"
		END "Total",
		CASE 
			WHEN SCL6."U_ENG_Tipo" = 'Atendimento KM' THEN SCL6."U_ENG_KM" * MULT."U_Mult"
			WHEN SCL6."U_ENG_Tipo" = 'Atendimento HR' THEN SCL6."ActualDur" * MULT."U_Mult"
			ELSE SCL6."U_ENG_NFValor" + (SCL6."U_ENG_NFValor" * MULT."U_Mult" / 100)
		END "A Receber",
		SCL6."U_ENG_NC" 	"NC",
		OSCL."U_ENG_NumWC" 	"WC",
		OSCL."custmrName" 	"Nome do Cliente",
		OSCL."createDate" 	"Data Abertura",
		OSCL."internalSN" 	"Série",
		CASE 
			WHEN SCL6."U_ENG_Pago" = 'Y' THEN 'Sim'
			ELSE 'Não'
		END "Pago",
		SCL6."U_ENG_GAR_LCM" "LCM"
		
	FROM OSCL
		INNER JOIN SCL6 ON SCL6."SrcvCallID" = OSCL."callID"
		LEFT JOIN "@ENG_SERV_MULT" MULT ON MULT."U_Tipo" = "U_ENG_Tipo" AND IFNULL(OSCL."U_ENG_DataWC", OSCL."createDate") BETWEEN MULT."U_VigenciaDe" AND MULT."U_VigenciaAte"
	WHERE (filterType = 'NC' AND "U_ENG_NC" = number)
	OR (filterType = 'WC' AND "U_ENG_NumWC" = number)
	ORDER BY OSCL."callID";
END; 
