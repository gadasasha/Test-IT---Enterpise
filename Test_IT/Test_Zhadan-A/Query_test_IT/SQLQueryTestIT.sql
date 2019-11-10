USE master
GO

CREATE DATABASE TestDB_001_IT
GO

USE TestDB_001_IT
GO

--  �������� ������

CREATE TABLE TOV
(
    KTOV char(10) NOT NULL PRIMARY KEY,
    NTOV char(60) NULL,
	SORT char(10) NULL
);

CREATE TABLE DMZ
(
    DDM date NOT NULL ,
    NDM INT  NOT NULL  PRIMARY KEY,
	PR  INT NULL
);

CREATE TABLE DMS
(
    NDM INT  NOT NULL FOREIGN KEY REFERENCES DMZ(NDM) ON DELETE CASCADE ON UPDATE CASCADE,
	KTOV char(10) NOT NULL FOREIGN KEY REFERENCES TOV(KTOV) ON DELETE CASCADE ON UPDATE CASCADE,
	KOL decimal(13,2) NULL,
	CENA decimal(13,2) NULL,
	SORT CHAR(10) NULL,
);


INSERT TOV VALUES
('101',    '����',    '�������'),
('102',    '����',    '������'),
('103',    '�����',    '� ��������')

INSERT DMZ VALUES
('01.05.2014',    '2',    '1'),
('01.05.2014',    '3',    '2'),
('02.05.2014',    '5',    '2')

INSERT DMS VALUES
('2',  '101',    '100',     '8.00',       '�������'),
('2',  '102',     '80',     '9.50',        '������'),
('2',  '103',     '50',     '6.50',    '� ��������'),
('3',  '101',      '1',    '10.00',       '�������'),
('3',  '103',      '1',     '8.50',    '� ��������'),
('3',  '101',      '2',    '10.00',       '�������'),
('3',  '102',      '1',    '11.50',        '������'),
('5',  '101',      '2',    '10.50',       '�������'),
('5',  '103',      '1',     '8.60',       '�������')

SELECT * FROM TOV
SELECT * FROM DMS
SELECT * FROM DMZ



-- �������
-- ������ �1. �����: � ������� "DMS" ����� �������� ���� "NDM"

-- ������ �2. �����: ������������ ��������, ���� ���������� ����� "SORT" � �������� "TOV" � "DMS" '������ ���������� �����'

-- ������ �3.1 �����:

SELECT NTOV, SUM(KOL) AS KOLD, SUM(KOL)*CENA AS ITOGO FROM TOV T 
	JOIN DMS D
	ON T.KTOV = D.KTOV
	JOIN DMZ Z
	ON D.NDM = Z.NDM
WHERE Z.DDM = '20140501' AND Z.NDM=3 GROUP BY  NTOV, DDM, CENA ORDER  BY ITOGO DESC;


-- ������ �3.2 �����:

UPDATE DMS
SET SORT = '������'
FROM TOV t
JOIN DMS s 
ON t.SORT=s.SORT
WHERE s.KTOV = 101 

-- ������ �3.3 �����:

SELECT(
SELECT NTOV, SUM(KOL) AS KOLD, SUM(KOL)*CENA AS ITOGO  FROM TOV T 
	JOIN DMS D
	ON T.KTOV = D.KTOV
	JOIN DMZ Z
	ON D.NDM = Z.NDM
WHERE  Z.PR=2  GROUP BY  NTOV, CENA ORDER  BY ITOGO DESC) AS F
 EXCEPT  
SELECT (
SELECT NTOV, SUM(KOL) AS KOLD, SUM(KOL)*CENA AS ITOGO  FROM TOV T 
	JOIN DMS D
	ON T.KTOV = D.KTOV
	JOIN DMZ Z
	ON D.NDM = Z.NDM
WHERE  Z.PR=1  GROUP BY  NTOV, CENA ORDER  BY ITOGO DESC) AS C
JOIN 

-- ������ �3.4 �����:
INSERT INTO DMZ (DDM, NDM, PR) 
VALUES (GETDATE(), (SELECT MAX(NDM) FROM DMZ ) + 1 , IIF( (SELECT SUM(PR) FROM DMZ WHERE PR=1 ) >= (SELECT SUM(PR)/2 FROM DMZ WHERE PR=2 ), 2,1) )

-- ������ �3.5 �����:
INSERT INTO DMS (NDM, KTOV, KOL, CENA, SORT)
VALUES ( 
(SELECT MAX(NDM)   
FROM DMS
EXCEPT  
SELECT MIN(NDM)   
FROM DMS  ) AS MAX1,

(SELECT MAX(KTOV)   
FROM DMS
EXCEPT  
SELECT MIN(KTOV)   
FROM DMS )

; 

SELECT MAX(NDM)   
FROM DMS
EXCEPT  
SELECT MIN(NDM)   
FROM DMS ;  



