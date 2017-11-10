#!/usr/bin/env python
# -*- coding: utf-8 -*-

from requests import Request
import json
import time
from pprint import pprint

from api_client.response_wrappers.viewtransactions_response_wrapper import ViewTransactionsResponseWrapper

class ViewTransactionsRequest(Request):

	URL = "https://investor-api.herokuapp.com/api/1.0/accounts/ACCOUNT_ID/transactions"

	def __init__(self, session, token, account_id, page_number = None, page_size = None,
		start_date = None, end_date = None):
		self.token = token
		self.account_id = account_id
		self.page_number = page_number
		self.page_size = page_size
		self.start_date = start_date
		self.end_date = end_date
		self.make_request()
		self.session = session

	def make_request(self):
		self.URL = self.URL.replace('ACCOUNT_ID', str(self.account_id))
		
		payload_data = {
			"pageNumber": self.page_number, 
			"pageSize": self.page_size, 
			"startDate": self.start_date, 
			"endDate": self.end_date
		}

		payload = json.dumps(payload_data, ensure_ascii = False).encode('utf8')
		
		headers = {
			"Content-Type": "application/json",
			"Authorization": "Bearer " + str(self.token)
		}
		super().__init__('GET', url = self.URL, headers = headers, params = payload)

	def get_response(self):
		response = self.session.send(super().prepare())
		wrapped_response = ViewTransactionsResponseWrapper(response)
		response.close()
		return wrapped_response