#!/usr/bin/env python
# -*- coding: utf-8 -*-

from requests import Request
import json
from pprint import pprint

from api_client.response_wrappers.historicalprices_response_wrapper import HistoricalPricesResponseWrapper

class HistoricalPricesRequest(Request):

	URL = "https://investor-api.herokuapp.com/api/1.0/shares/SYMBOL/prices"

	def __init__(self, session, token, symbol, end_time = None,
		interval = None, date_range = None):

		self.token = token
		self.symbol = symbol
		self.end_time = end_time
		self.interval = interval
		self.date_range = date_range
		self.make_request()
		self.session = session

	def make_request(self):
		self.URL = self.URL.replace('SYMBOL', str(self.symbol))
		payload_data = {}
		if self.end_time is not None:
			payload_data["endTime"] = self.end_time
		if self.interval is not None:
			payload_data["interval"] = self.interval
		if self.date_range is not None:
			payload_data["range"] = self.date_range

		pprint(self.URL)
		#pprint(payload_data)

		headers = {
			"Content-Type": "application/json",
			"Authorization": "Bearer " + str(self.token)
		}
		#pprint(headers)
		super().__init__('GET', url = self.URL, headers = headers, params = payload_data)

	def get_response(self):
		response = self.session.send(super().prepare())
		wrapped_response = HistoricalPricesResponseWrapper(response)
		response.close()
		return wrapped_response