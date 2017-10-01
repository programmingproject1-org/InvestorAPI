#!/usr/bin/env python
# -*- coding: utf-8 -*-

from requests import Request
import json

from api_client.response_wrappers.historicalprices_response_wrapper import HistoricalPricesResponseWrapper

class HistoricalPricesRequest(Request):

	URL = "https://investor-api.herokuapp.com/api/1.0/shares/SYMBOL/prices"

	def __init__(self, session, token, symbol, start_time = None,
		end_time = None, interval = None, date_range = None):

		self.token = token
		self.symbol = symbol
		self.start_time = start_time
		self.end_time = end_time
		self.interval = interval
		self.date_range = date_range
		self.make_request()
		self.session = session

	def make_request(self):
		self.URL = self.URL.replace('SYMBOL', str(self.symbol))
		payload_data = {}
		if self.start_time is not None:
			payload_data["start_time"] = self.start_time
		if self.end_time is not None:
			payload_data["end_time"] = self.end_time
		if self.interval is not None:
			payload_data["interval"] = self.interval
		if self.date_range is not None:
			payload_data["date_range"] = self.date_range

		payload = json.dumps(payload_data, ensure_ascii = False).encode('utf8')

		headers = {
			"Content-Type": "application/json",
			"Authorization": "Bearer " + str(self.token)
		}

		super().__init__('GET', url = self.URL, headers = headers, data = payload)

	def get_response(self):
		response = self.session.send(super().prepare())
		wrapped_response = HistoricalPricesResponseWrapper(response)
		response.close()
		return wrapped_response