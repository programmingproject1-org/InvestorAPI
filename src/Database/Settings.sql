-- Table: public."Settings"

-- DROP TABLE public."Settings";

CREATE TABLE public."Settings"
(
    "Key" character varying(30) COLLATE pg_catalog."default" NOT NULL,
    "Value" text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "Settings_pkey" PRIMARY KEY ("Key")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."Settings"
    OWNER to wmjepiakfzqvwv;
