#!/usr/bin/env python
# -*- coding: utf-8 -*-

import json
from pprint import pprint

class ViewTransactionsResponseWrapper():

	SUCCESS_STATUS = 200

	def __init__(self, response):
		self.response = response

	def get_http_status(self):
		return self.response.status_code

	def get_json_body(self):
		try:
			body = self.response.json()
		except ValueError:
			body = None
		return body

	def get_items(self):
		return self.get_json_body()["items"]

	def get_page_number(self):
		return self.get_json_body()["pageNumber"]

	def get_page_size(self):
		return self.get_json_body()["pageSize"]

	def get_total_page_count(self):
		return self.get_json_body()["totalPageCount"]

	def get_total_row_count(self):
		return self.get_json_body()["totalRowCount"]