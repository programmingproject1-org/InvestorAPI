-- Table: public."Transactions"

-- DROP TABLE public."Transactions";

CREATE TABLE public."Transactions"
(
    "Id" uuid NOT NULL,
    "AccountId" uuid NOT NULL,
    "Description" text COLLATE pg_catalog."default" NOT NULL,
    "Amount" money NOT NULL,
    "TimestampUtc" timestamp without time zone NOT NULL,
    "Balance" money NOT NULL,
    CONSTRAINT "Transactions_pkey" PRIMARY KEY ("Id"),
    CONSTRAINT "Transactions_Accounts" FOREIGN KEY ("AccountId")
        REFERENCES public."Accounts" ("Id") MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE CASCADE
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."Transactions"
    OWNER to wmjepiakfzqvwv;

-- Index: fki_Transactions_Accounts

-- DROP INDEX public."fki_Transactions_Accounts";

CREATE INDEX "fki_Transactions_Accounts"
    ON public."Transactions" USING btree
    (AccountId)
    TABLESPACE pg_default;
