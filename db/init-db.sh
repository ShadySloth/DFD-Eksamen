#!/bin/bash
echo "host    all             all             172.31.0.1          md5" >> /var/lib/postgresql/data/pg_hba.conf
pg_ctl reload